using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    [Table("timeline")]
    public partial class Timeline
    {
        [Display(Name = "时间线Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimelineId { get; set; }
        [Display(Name = "创建时间")]
        public DateTime TimelineCreateTime { get; set; }
        [Display(Name = "时间线标题")]
        [StringLength(100)]
        public string TimelineTitle { get; set; }
        [Display(Name = "时间线内容")]
        public string TimtlineContent { get; set; }
    }
}
