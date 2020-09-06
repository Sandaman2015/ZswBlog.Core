using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    [Table("comment")]
    public partial class Comment
    {
        [Display(Name = "评论Id")]
        [Key]
        public int CommentId { get; set; }
        [Display(Name = "留言内容")]
        [StringLength(500)]
        public string Comment1 { get; set; }
        [Display(Name = "评论日期")]
        public DateTime CommentDate { get; set; }
        [Display(Name = "用户Id")]
        [Column("UserId")]
        public Guid UserId { get; set; }
        [Display(Name = "评论目标用户Id")]
        [Column("TargetUserId")]
        public Guid? TargetUserId { get; set; }
        [Display(Name = "评论父Id:有默认值0")]
        [Column("TargetCommentId")]
        public int TargetId { get; set; } = 0;
        [Display(Name = "评论位置")]
        [StringLength(50)]
        public string Location { get; set; }
        [Display(Name = "浏览器")]
        [StringLength(50)]
        public string Browser { get; set; }
        [Display(Name = "文章Id")]
        public int ArticleId { get; set; }
    }
}
