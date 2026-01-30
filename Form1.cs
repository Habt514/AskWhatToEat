using SpriteDrawer;
using System;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace AskWhatToEat
{
    public partial class FormAsk : Form
    {
        public FormAsk()
        {
            InitializeComponent();
        }

        private void FormAsk_Load(object sender, EventArgs e)
        {
            // 原有信息提示
            //MessageBox.Show($"当前用户: {GetLocalUserName()}\n当前时间: {GetCurrentDateString()}", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 示例：使用 当前用户名 + 当前日期（按 yyyy-MM-dd） 作为种子，在 [1,100] 范围内生成确定性随机数（含1，不含101）
            int deterministic = DeterministicRandomForCurrent(1, 101);
            //MessageBox.Show($"基于 用户+日期 的确定性随机数: {deterministic}", "示例", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }
        private static string GetLocalUserName()
        {
            return Environment.UserName ?? string.Empty;
        }

        /// <summary>
        /// 获取当前日期时间字符串
        /// - utc: 是否使用 UTC
        /// - format: 自定义格式（默认 "yyyy-MM-dd"）
        /// - culture: 指定文化，默认使用当前文化
        /// </summary>
        private static string GetCurrentDateString(bool utc = false, string? format = null, CultureInfo? culture = null)
        {
            var dt = utc ? DateTimeOffset.UtcNow : DateTimeOffset.Now;
            format ??= "yyyy-MM-dd";
            culture ??= CultureInfo.CurrentCulture;
            return dt.ToString(format, culture);
        }

        /// <summary>
        /// 根据 用户名 和 日期字符串 生成一个 int 种子（从 SHA-256 哈希派生，保证稳定且均匀）
        /// </summary>
        private static int ComputeSeedFromUserAndDate(string userName, string dateString)
        {
            var input = $"{userName}|{dateString}";
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            // 取哈希的前4字节，转换为无符号整数，再确保为非负的 int
            uint val = BitConverter.ToUInt32(hash, 0);
            int seed = (int)(val & 0x7FFFFFFF);
            return seed;
        }

        /// <summary>
        /// 使用指定 username 和 dateString 作为种子生成确定性随机数，范围 [minInclusive, maxExclusive)
        /// 抛出 ArgumentException 当 minInclusive &gt;= maxExclusive
        /// </summary>
        public static int DeterministicRandom(string userName, string dateString, int minInclusive, int maxExclusive)
        {
            if (minInclusive >= maxExclusive) throw new ArgumentException("minInclusive 必须小于 maxExclusive");
            int seed = ComputeSeedFromUserAndDate(userName ?? string.Empty, dateString ?? string.Empty);
            var rnd = new Random(seed);
            return rnd.Next(minInclusive, maxExclusive);
        }

        /// <summary>
        /// 使用当前 Environment.UserName 与当前日期（默认格式 yyyy-MM-dd）生成确定性随机数
        /// - useDateOnly: true 使用日期（yyyy-MM-dd），false 使用到秒（yyyy-MM-dd HH:mm:ss）
        /// </summary>
        public static int DeterministicRandomForCurrent(int minInclusive = 0,int maxExclusive=0, bool useDateOnly = true)
        {
            var user = Environment.UserName ?? string.Empty;
            var dateFormat = useDateOnly ? "yyyy-MM-dd" : "yyyy-MM-dd HH:mm:ss";
            var dateStr = GetCurrentDateString(false, dateFormat);
            return DeterministicRandom(user, dateStr, minInclusive, maxExclusive);
        }
        public static string ResolveRelativePath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) throw new ArgumentNullException(nameof(relativePath));
            // 优先使用运行时基目录（可在发布和调试时都可靠）
            string baseDir = AppContext.BaseDirectory
                             ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                             ?? Application.StartupPath;
            return Path.GetFullPath(Path.Combine(baseDir, relativePath));
        }
        private void ask_Click(object sender, EventArgs e)
        {
            int drinkNum = DeterministicRandomForCurrent(minInclusive: 0, maxExclusive: 45, useDateOnly:false);
            int foodNum = DeterministicRandomForCurrent(minInclusive: 0, maxExclusive: 164, useDateOnly: false);
            SpriteDrawer.SpriteDrawer spriteDrawer = new SpriteDrawer.SpriteDrawer();
            foodPicture.Image = spriteDrawer.SpriteDraw(Properties.Resources.foods, foodNum, zoom: 6);
            drinkPicture.Image = spriteDrawer.SpriteDraw(Properties.Resources.drinks, drinkNum, zoom: 6);
            string? theFoodName = GetFromxlsx.GetFromxlsx.GetCellString(ResolveRelativePath(relativePath: @"Data\\foods.xlsx"), 1, foodNum+1, 1); // sheet1, row2,col3
            foodName.Text = theFoodName ?? "<null>";
            string? theFoodPrice = GetFromxlsx.GetFromxlsx.GetCellString(ResolveRelativePath(relativePath: @"Data\\foods.xlsx"), 1, foodNum+1, 2);
            foodPrice.Text = theFoodPrice ?? "<null>";
            string? theDrinkName = GetFromxlsx.GetFromxlsx.GetCellString(ResolveRelativePath(relativePath: @"Data\\drinks.xlsx"), 1, drinkNum+1, 1); // sheet1, row2,col3
            drinkName.Text = theDrinkName ?? "<null>";
            string? theDrinkPrice = GetFromxlsx.GetFromxlsx.GetCellString(ResolveRelativePath(relativePath: @"Data\\drinks.xlsx"), 1, drinkNum +1, 2);
            drinkPrice.Text = theDrinkPrice ?? "<null>";
            //MessageBox.Show($"drinkNum:{drinkNum}\nfoodNum{foodNum}");
        }
    }
}
