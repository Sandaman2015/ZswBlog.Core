using System;

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
        public int MessageId { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 留言用户id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 留言时间
        /// </summary>
        public DateTime MessageDate { get; set; }

        /// <summary>
        /// 用户浏览器
        /// </summary>
        public string Browser { get; set; }
        /// <summary>
        /// 用户地址
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserPortrait { get; set; }
        /// <summary>
        /// 目标用户名称
        /// </summary>
        public string TargetUserName { get; set; }
    }
}
