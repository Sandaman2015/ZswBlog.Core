using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 文章评论
    /// </summary>
    [Table("comment")]
    public partial class Comment
    {
        /// <summary>
        /// 评论Id
        /// </summary>
        [Display(Name = "评论Id")]
        [Key]
        public int CommentId { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        [Display(Name = "留言内容")]
        [StringLength(500)]
        public string Comment1 { get; set; }
        /// <summary>
        /// 评论日期
        /// </summary>
        [Display(Name = "评论日期")]
        public DateTime CommentDate { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        [Display(Name = "用户Id")]
        [Column("UserId")]
        public Guid UserId { get; set; }
        /// <summary>
        /// 评论目标用户Id
        /// </summary>
        [Display(Name = "评论目标用户Id")]
        [Column("TargetUserId")]
        public Guid? TargetUserId { get; set; }
        /// <summary>
        /// 评论父Id:有默认值0
        /// </summary>
        [Display(Name = "评论父Id:有默认值0")]
        [Column("TargetCommentId")]
        public int TargetId { get; set; } = 0;
        /// <summary>
        /// 用户位置
        /// </summary>
        [Display(Name = "评论位置")]
        [StringLength(50)]
        public string Location { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>
        [Display(Name = "浏览器")]
        [StringLength(50)]
        public string Browser { get; set; }
        /// <summary>
        /// 所属文章Id
        /// </summary>
        [Display(Name = "文章Id")]
        public int ArticleId { get; set; }
    }
}
