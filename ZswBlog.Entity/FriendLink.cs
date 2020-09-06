using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    [Table("friendlink")]
    public partial class FriendLink
    {
        [Display(Name = "友链Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FriendlinkId { get; set; }
        [Display(Name = "友链标题")]
        [Required()]
        [StringLength(50)]
        public string FriendlinkTitle { get; set; }
        [Display(Name = "友链创建时间")]
        [Required()]
        public DateTime FriendlinkCreateTime { get; set; }
        [Display(Name = "友链的图片")]
        [StringLength(350)]
        public string FriendlinkImage { get; set; }
        [Display(Name = "友链链接")]
        [StringLength(100)]
        public string Friendlink { get; set; }
        [Display(Name = "是否显示友链")]
        [DefaultValue(0)]
        public int IsShow { get; set; } = 0;
        [Display(Name = "友链介绍")]
        [StringLength(80)]
        public string FriendlinkIntroduce { get; set; }
    }
}
