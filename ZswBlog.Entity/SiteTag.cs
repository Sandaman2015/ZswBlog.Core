using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 站点标签
    /// </summary>
    [Table("sitetag")]
    public partial class SiteTag
    {
        /// <summary>
        /// 站点标签Id
        /// </summary>
        [Display(Name = "站点标签Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SitetagId { get; set; }
        /// <summary>
        /// 站点标题
        /// </summary>
        [Display(Name = "站点标题")]
        [StringLength(20)]
        public string SitetagTitle { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        [Display(Name = "点赞数")]
        public int SitetagLikes { get; set; } = 0;
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime SitetagCreateTime { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        [Display(Name = "用户Id")]
        public Guid UserId { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        [Display(Name = "是否显示")]
        public bool IsShow { get; set; } = false;
    }
}
