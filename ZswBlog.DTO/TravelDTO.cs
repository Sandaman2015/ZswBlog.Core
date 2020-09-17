using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.DTO
{
    /// <summary>
    /// 旅行DTO
    /// </summary>
    public class TravelDTO
    {
        /// <summary>
        /// 旅行分享id
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
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 旅行分享内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 是否显示1不显示,0显示
        /// </summary>
        public bool isShow { get; set; }
    }
}
