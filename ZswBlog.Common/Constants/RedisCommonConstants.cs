namespace ZswBlog.Common.Constants
{
    public struct RedisCommonConstants
    {
        /// <summary>
        /// 默认过期时间 -1
        /// </summary>
        public static readonly int DefaultExpress = -1;
        /// <summary>
        /// 通用过期时间 60
        /// </summary>
        public static readonly int CommonExpress = 60;
        /// <summary>
        /// 时过期时间 3600
        /// </summary>
        public static readonly int HourExpress = 60 * 60;
        /// <summary>
        /// 日过期时间 86400
        /// </summary>
        public static readonly int DayExpress = 60 * 60 * 24;
        /// <summary>
        /// 周过期时间 604,800
        /// </summary>
        public static readonly int WeekExpress = 60 * 60 * 24 * 7;
        /// <summary>
        /// 月过期时间 2,592,000
        /// </summary>
        public static readonly int MonthExpress = 60 * 60 * 24 * 30;
        /// <summary>
        /// 月过期时间 31,104,000
        /// </summary>
        public static readonly int YearExpress = 60 * 60 * 24 * 30 * 12;
        /// <summary>
        /// 十秒过期时间 10
        /// </summary>
        public static readonly int TenExpress = 10;
        /// <summary>
        /// 百秒过期时间 100
        /// </summary>
        public static readonly int HundredExpress = 100;
        /// <summary>
        /// 千秒过期时间 1000
        /// </summary>
        public static readonly int ThousandExpress = 1000;
    }
}