using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 留言
    /// </summary>
    [Table("message")]
    public partial class Message
    {
        /// <summary>
        /// 留言Id
        /// </summary>
        [Display(Name = "留言Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        [Display(Name = "留言内容")]
        [StringLength(500)]
        public string Message1 { get; set; }
        /// <summary>
        /// 留言日期
        /// </summary>
        [Display(Name = "留言日期")]
        public DateTime MessageDate { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        [Display(Name = "用户Id")]
        [Column("UserId")]
        public Guid UserId { get; set; }
        /// <summary>
        /// 留言目标用户Id
        /// </summary>
        [Display(Name = "留言目标用户Id")]
        [Column("TargetUserId")]
        public Guid? TargetUserId { get; set; }
        /// <summary>
        /// 留言父Id:有默认值0
        /// </summary>
        [Display(Name = "留言父Id:有默认值0")]
        [Column("TargetMessageId")]
        public int TargetId { get; set; } = 0;
        /// <summary>
        /// 留言位置
        /// </summary>
        [Display(Name = "用户位置")]
        [StringLength(50)]
        public string Location { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>
        [Display(Name = "用户浏览器")]
        [StringLength(50)]
        public string Browser { get; set; }
    }
}
