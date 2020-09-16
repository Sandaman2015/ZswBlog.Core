using System;

namespace ZswBlog.DTO
{
    /// <summary>
    /// 时间线对象
    /// </summary>
    public partial class TimeLineDTO
    {
        /// <summary>
        /// 时间线id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 时间线内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 时间线标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 时间线时间
        /// </summary>
        public DateTime createDate { get; set; }
    }
}
