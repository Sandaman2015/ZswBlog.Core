using System;

namespace ZswBlog.DTO
{
    public partial class CommentDTO
    {
        /// <summary>
        /// 评论id
        /// </summary>
        public int CommentId { get; set; }
        /// <summary>
        /// 评论
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime CommentDate { get; set; }
        /// <summary>
        /// 用户浏览器
        /// </summary>
        public string Browser { get; set; }
        /// <summary>
        /// 用户定位
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 评论用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 评论用户头像
        /// </summary>
        public string UserPortrait { get; set; }
        /// <summary>
        /// 评论目标用户名称
        /// </summary>
        public string TargetUserName { get; set; }
    }
}
