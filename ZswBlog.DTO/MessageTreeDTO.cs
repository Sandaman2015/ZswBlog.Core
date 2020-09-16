using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.DTO
{
    /// <summary>
    /// 留言树DTO
    /// </summary>
    public partial class MessageTreeDTO
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

        //-----------------------逻辑关联字段
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

        /// <summary>
        /// 留言子集
        /// </summary>
        public List<MessageTreeDTO> children { get; set; }
    }
}
