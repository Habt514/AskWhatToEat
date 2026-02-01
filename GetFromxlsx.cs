using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ClosedXML.Excel;

namespace GetFromxlsx
{
    public class GetFromxlsx
    {
        /// <summary>
        /// 从指定文件的指定工作表和单元格地址（例如 "A1"）读取字符串。找不到或为空时返回 null。
        /// </summary>
        public static string? GetCellString(string filePath, string sheetName, string cellAddress)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("filePath 不能为空", nameof(filePath));
            if (!File.Exists(filePath)) throw new FileNotFoundException("文件未找到", filePath);
            if (string.IsNullOrWhiteSpace(cellAddress)) throw new ArgumentException("cellAddress 不能为空", nameof(cellAddress));

            using var wb = new XLWorkbook(filePath);
            IXLWorksheet ws;
            if (!string.IsNullOrEmpty(sheetName) && wb.Worksheets.TryGetWorksheet(sheetName, out ws))
            {
                // 使用指定名称的工作表
            }
            else
            {
                // 如果没有指定或找不到名称，则使用第一个工作表
                ws = wb.Worksheets.Worksheet(1);
            }

            var cell = ws.Cell(cellAddress);
            if (cell == null || cell.IsEmpty()) return null;
            var value = cell.GetString();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        /// <summary>
        /// 从指定文件的第 n 个工作表（1 基）和行/列读取字符串。找不到或为空时返回 null。
        /// </summary>
        public static string? GetCellString(string filePath, int sheetIndex, int row, int column)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("filePath 不能为空", nameof(filePath));
            if (!File.Exists(filePath)) throw new FileNotFoundException("文件未找到", filePath);
            if (sheetIndex < 1) throw new ArgumentOutOfRangeException(nameof(sheetIndex), "sheetIndex 必须 >= 1");
            if (row < 1) throw new ArgumentOutOfRangeException(nameof(row), "row 必须 >= 1");
            if (column < 1) throw new ArgumentOutOfRangeException(nameof(column), "column 必须 >= 1");

            using var wb = new XLWorkbook(filePath);
            if (sheetIndex > wb.Worksheets.Count) throw new ArgumentOutOfRangeException(nameof(sheetIndex), "指定的工作表索引超出范围");
            var ws = wb.Worksheets.Worksheet(sheetIndex);

            var cell = ws.Cell(row, column);
            if (cell == null || cell.IsEmpty()) return null;
            var value = cell.GetString();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        // -----------------------------
        // 嵌入资源读取的新增方法
        // -----------------------------

        /// <summary>
        /// 从嵌入资源中的 .xlsx 读取指定工作表和单元格（通过 sheetName + cellAddress）。找不到或为空时返回 null。
        /// resourceName 可以是完整资源名或文件名（方法会尝试通过 EndsWith 匹配）。
        /// </summary>
        public static string? GetCellStringFromEmbeddedResource(string resourceName, string sheetName, string cellAddress)
        {
            if (string.IsNullOrWhiteSpace(resourceName)) throw new ArgumentException("resourceName 不能为空", nameof(resourceName));
            if (string.IsNullOrWhiteSpace(cellAddress)) throw new ArgumentException("cellAddress 不能为空", nameof(cellAddress));

            using var stream = GetEmbeddedResourceStream(resourceName);
            using var wb = new XLWorkbook(stream);
            IXLWorksheet ws;
            if (!string.IsNullOrEmpty(sheetName) && wb.Worksheets.TryGetWorksheet(sheetName, out ws))
            {
                // 指定名称
            }
            else
            {
                ws = wb.Worksheets.Worksheet(1);
            }

            var cell = ws.Cell(cellAddress);
            if (cell == null || cell.IsEmpty()) return null;
            var value = cell.GetString();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        /// <summary>
        /// 从嵌入资源中的 .xlsx 读取指定工作表（索引1基）和行/列。找不到或为空时返回 null。
        /// </summary>
        public static string? GetCellStringFromEmbeddedResource(string resourceName, int sheetIndex, int row, int column)
        {
            if (string.IsNullOrWhiteSpace(resourceName)) throw new ArgumentException("resourceName 不能为空", nameof(resourceName));
            if (sheetIndex < 1) throw new ArgumentOutOfRangeException(nameof(sheetIndex), "sheetIndex 必须 >= 1");
            if (row < 1) throw new ArgumentOutOfRangeException(nameof(row), "row 必须 >= 1");
            if (column < 1) throw new ArgumentOutOfRangeException(nameof(column), "column 必须 >= 1");

            using var stream = GetEmbeddedResourceStream(resourceName);
            using var wb = new XLWorkbook(stream);
            if (sheetIndex > wb.Worksheets.Count) throw new ArgumentOutOfRangeException(nameof(sheetIndex), "指定的工作表索引超出范围");
            var ws = wb.Worksheets.Worksheet(sheetIndex);

            var cell = ws.Cell(row, column);
            if (cell == null || cell.IsEmpty()) return null;
            var value = cell.GetString();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private static Stream GetEmbeddedResourceStream(string resourceName)
        {
            var asm = Assembly.GetExecutingAssembly();
            var names = asm.GetManifestResourceNames();
            // 先尝试 EndsWith 匹配（对常见情况更友好），否则使用传入的 resourceName 直接查找
            var match = names.FirstOrDefault(n => n.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase))
                        ?? names.FirstOrDefault(n => n.Equals(resourceName, StringComparison.Ordinal))
                        ?? resourceName;
            var stream = asm.GetManifestResourceStream(match);
            if (stream == null)
            {
                throw new FileNotFoundException($"嵌入资源未找到: '{resourceName}'. 可用资源: {string.Join(", ", names)}");
            }
            return stream;
        }
    }
}
