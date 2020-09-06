using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    [Table("message")]
    public partial class Message
    {
        [Display(Name = "留言Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        [Display(Name = "留言内容")]
        [StringLength(500)]
        public string Message1 { get; set; }
        [Display(Name = "留言日期")]
        public DateTime MessageDate { get; set; }
        [Display(Name = "用户Id")]
        [Column("UserId")]
        public Guid UserId { get; set; }
        [Display(Name = "留言目标用户Id")]
        [Column("TargetUserId")]
        public Guid? TargetUserId { get; set; }
        [Display(Name = "留言父Id:有默认值0")]
        [Column("TargetMessageId")]
        public int TargetId { get; set; } = 0;
        [Display(Name = "留言位置")]
        [StringLength(50)]
        public string Location { get; set; }
        [Display(Name = "浏览器")]
        [StringLength(50)]
        public string Browser { get; set; }
    }
}
