using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    [Table("sitetag")]
    public partial class SiteTag
    {
        [Display(Name = "站点标签Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SitetagId { get; set; }
        [Display(Name = "标题")]
        [StringLength(20)]
        public string SitetagTitle { get; set; }
        [Display(Name = "点赞数")]
        public int SitetagLikes { get; set; } = 0;
        [Display(Name = "创建时间")]
        public DateTime SitetagCreateTime { get; set; }
        [Display(Name = "用户Id")]
        public Guid UserId { get; set; }
        [Display(Name = "是否显示")]
        public int IsShow { get; set; } = 0;
    }
}
