using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.DTO
{
    /// <summary>
    /// 评论DTO
    /// </summary>
    public partial class CommentDTO
    {
        /// <summary>
        /// 评论id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createDate { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [NotMapped]
        public int userId { get; set; }
        /// <summary>
        /// 所属文章id
        /// </summary>
        public int articleId { get; set; }
        /// <summary>
        /// 目标用户
        /// </summary>
        [NotMapped]
        public int targetUserId { get; set; }
        /// <summary>
        /// 目标评论
        /// </summary>
        public int targetId { get; set; }
        /// <summary>
        /// 评论位置
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>
        public string browser { get; set; }

        //------------------------------关联查询填充的数据
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
        /// 映射对象
        /// </summary>
        [NotMapped]
        public virtual UserDTO user { get; set; }
        /// <summary>
        /// 映射对象
        /// </summary>
        [NotMapped]
        public virtual UserDTO targetUser { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        [NotMapped]
        public Nullable<bool> isShow { get; set; }
    }
}
