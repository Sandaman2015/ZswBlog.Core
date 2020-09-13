using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 时间线实体
    /// </summary>
    public class TimeLineEntity
    {
        /// <summary>
        /// 时间线id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createDate { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public int operatorId { get; set; }
        /// <summary>
        /// 时间线标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 时间线内容
        /// </summary>
        public string content { get; set; }
    }
}
