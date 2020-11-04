using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.DTO
{
    /// <summary>
    /// 留言对象
    /// </summary>
    public partial class MessageDTO
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
        [NotMapped]
        public int targetUserId { get; set; }
        /// <summary>
        /// 目标留言id
        /// </summary>
        [NotMapped]
        public int targetId { get; set; }
        /// <summary>
        /// 留言位置
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>
        public string browser { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string userPortrait { get; set; }
        /// <summary>
        /// 目标用户名称
        /// </summary>
        public string targetUserName { get; set; }

        /// <summary>
        /// 目标用户头像
        /// </summary>
        public string targetUserPortrait { get; set; }
    }
}
