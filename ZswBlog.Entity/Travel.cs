using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    [Table("travel")]
    public partial class Travel
    {
        [Display(Name = "旅行Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TravelId { get; set; }
        [Display(Name = "旅行标题")]
        [StringLength(50)]
        public string TravelTitle { get; set; }
        [Display(Name = "创建时间")]
        public DateTime TravelCreateTime { get; set; }
        [Display(Name = "旅行外链")]
        [StringLength(200)]
        public string TravelLink { get; set; }
        [Display(Name = "旅行插图")]
        [StringLength(300)]
        public string TravelImage { get; set; }
    }
}
