using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SpriteDrawer
{
    /// <summary>
    /// 从精灵图中提取单帧图像的工具类。
    /// 构造时可指定单帧宽高、放大倍数，以及可选的精灵总数和每行精灵数（列数）。
    /// 调用 SpriteDraw 返回指定索引的精灵（已放大）。
    /// </summary>
    public class SpriteDrawer
    {
        public int SingleWidth { get; }
        public int SingleHeight { get; }
        public int Zoom { get; }

        /// <summary>
        /// 如果为 0 表示在绘制时根据图片自动推断（保持向后兼容）。
        /// 如果指定了 SpriteNum 和/或 LNum，SpriteDraw 会尽量以这些值为准：
        /// - 当 SpriteNum>0 且 LNum>0：按给定列数 LNum 和总数 SpriteNum 计算行数。
        /// - 当 SpriteNum>0 且 LNum==0：按图片宽度推断列数，再按 SpriteNum 计算行数。
        /// - 当 SpriteNum==0 且 LNum>0：按给定列数 LNum，但总帧数以图片可容纳的格子数为准。
        /// - 当两者均为 0：完全按图片尺寸计算列/行/总数（原有行为）。
        /// </summary>
        public int SpriteNum { get; }
        public int LNum { get; }

        // 修复点：将 lNum 的默认值改为 0（表示按图片自动推断）
        public SpriteDrawer(int singleWidth = 26, int singleHeight = 26, int zoom = 1, int spriteNum = 0, int lNum = 0)
        {
            if (singleWidth <= 0) throw new ArgumentOutOfRangeException(nameof(singleWidth));
            if (singleHeight <= 0) throw new ArgumentOutOfRangeException(nameof(singleHeight));
            if (zoom <= 0) throw new ArgumentOutOfRangeException(nameof(zoom));
            if (spriteNum < 0) throw new ArgumentOutOfRangeException(nameof(spriteNum));
            if (lNum < 0) throw new ArgumentOutOfRangeException(nameof(lNum));

            SingleWidth = singleWidth;
            SingleHeight = singleHeight;
            Zoom = zoom;
            SpriteNum = spriteNum;
            LNum = lNum;
        }

        /// <summary>
        /// 提取精灵图中索引为 n 的帧并返回为新的 Image。
        /// 如果 n 超出上限（n >= 总帧数）则回到第一个（索引 0）；负索引仍按循环归一化。
        /// 返回的 Image 大小为 SingleWidth*effectiveZoom × SingleHeight*effectiveZoom。
        /// 使用最近邻插值以保留像素风格。
        /// </summary>
        /// <param name="picture">包含精灵的整张图片（非 null）</param>
        /// <param name="n">帧索引（从 0 开始，可以为任意整数）</param>
        /// <param name="zoom">
        /// 可选：用于本次绘制的放大倍数。若 <= 0 则使用构造时的实例级 Zoom 值（向后兼容）。
        /// </param>
        /// <returns>新的 Image（调用方负责释放）</returns>
        public Image SpriteDraw(Image picture, int n, int zoom = -1)
        {
            if (picture == null) throw new ArgumentNullException(nameof(picture));
            if (picture.Width < SingleWidth || picture.Height < SingleHeight)
                throw new ArgumentException("精灵图尺寸小于单帧尺寸。", nameof(picture));

            // 计算本次使用的放大倍数：如果传入的 zoom <= 0 则使用实例 Zoom
            int effectiveZoom = zoom > 0 ? zoom : Zoom;
            if (effectiveZoom <= 0) throw new ArgumentOutOfRangeException(nameof(zoom), "放大倍数必须大于 0。");

            // 自动推断的列/行/总数（基于图片）
            int autoColumns = picture.Width / SingleWidth;
            int autoRows = picture.Height / SingleHeight;
            int autoTotal = autoColumns * autoRows;

            if (autoColumns <= 0 || autoRows <= 0)
                throw new ArgumentException("精灵图尺寸不符合单帧宽高。", nameof(picture));

            int columns, rows, total;

            if (SpriteNum > 0 && LNum > 0)
            {
                // 使用用户指定的总数和列数
                columns = LNum;
                total = SpriteNum;
                rows = (total + columns - 1) / columns;

                if (picture.Width < columns * SingleWidth || picture.Height < rows * SingleHeight)
                    throw new ArgumentException("图片尺寸不足以容纳指定的列数或行数。", nameof(picture));
            }
            else if (SpriteNum > 0 && LNum == 0)
            {
                // 总数指定、列数按图片推断
                columns = autoColumns;
                total = SpriteNum;
                rows = (total + columns - 1) / columns;

                if (picture.Height < rows * SingleHeight)
                    throw new ArgumentException("图片高度不足以容纳指定的精灵总数。", nameof(picture));
            }
            else if (SpriteNum == 0 && LNum > 0)
            {
                // 列数指定、总数按图片推断
                columns = LNum;
                rows = autoRows;
                total = autoTotal;

                if (picture.Width < columns * SingleWidth)
                    throw new ArgumentException("图片宽度不足以容纳指定的列数。", nameof(picture));
            }
            else
            {
                // 均未指定：完全按图片推断（原有行为）
                columns = autoColumns;
                rows = autoRows;
                total = autoTotal;
            }

            if (columns <= 0 || rows <= 0 || total == 0)
                throw new ArgumentException("精灵图尺寸不符合单帧宽高或指定参数无效。", nameof(picture));

            // 索引归一化：
            // - 当 n >= total 时直接回到第一个（索引 0）。
            // - 负索引及小于上限的正索引按模运算归一化。
            int idx;
            if (n >= total)
            {
                idx = 0;
            }
            else
            {
                idx = ((n % total) + total) % total;
            }

            int col = idx % columns;
            int row = idx / columns;

            Rectangle srcRect = new Rectangle(col * SingleWidth, row * SingleHeight, SingleWidth, SingleHeight);
            int destW = SingleWidth * effectiveZoom;
            int destH = SingleHeight * effectiveZoom;

            var bmp = new Bitmap(destW, destH, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.SmoothingMode = SmoothingMode.None;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.Clear(Color.Transparent);
                g.DrawImage(picture, new Rectangle(0, 0, destW, destH), srcRect, GraphicsUnit.Pixel);
            }

            return bmp;
        }
    }
}