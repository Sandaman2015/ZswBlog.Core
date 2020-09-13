using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 留言实体对象
    /// </summary>
    public class MessageEntity
    {
        /// <summary>
        /// 留言id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 留言时间
        /// </summary>
        public DateTime createDate { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 目标留言用户
        /// </summary>
        public int targetUserId { get; set; }
        /// <summary>
        /// 目标留言id
        /// </summary>
        public int targetId { get; set; }
        /// <summary>
        /// 留言位置
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>
        public string browser { get; set; }
    }
}
