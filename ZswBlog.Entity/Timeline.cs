using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 时间线
    /// </summary>
    [Table("timeline")]
    public partial class Timeline
    {
        /// <summary>
        /// 时间线Id
        /// </summary>
        [Display(Name = "时间线Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimelineId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime TimelineCreateTime { get; set; }
        /// <summary>
        /// 时间线标题
        /// </summary>
        [Display(Name = "时间线标题")]
        [StringLength(100)]
        public string TimelineTitle { get; set; }
        /// <summary>
        /// 时间线内容
        /// </summary>
        [Display(Name = "时间线内容")]
        public string TimtlineContent { get; set; }
    }
}
