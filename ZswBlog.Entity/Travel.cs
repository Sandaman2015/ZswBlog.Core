using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 旅行
    /// </summary>
    [Table("travel")]
    public partial class Travel
    {
        /// <summary>
        /// 旅行Id
        /// </summary>
        [Display(Name = "旅行Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TravelId { get; set; }
        /// <summary>
        /// 旅行标题
        /// </summary>
        [Display(Name = "旅行标题")]
        [StringLength(50)]
        public string TravelTitle { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime TravelCreateTime { get; set; }
        /// <summary>
        /// 旅行外链
        /// </summary>
        [Display(Name = "旅行外链")]
        [StringLength(200)]
        public string TravelLink { get; set; }
        /// <summary>
        /// 旅行插图
        /// </summary>
        [Display(Name = "旅行插图")]
        [StringLength(300)]
        public string TravelImage { get; set; }
        //TODO 该实体需要添加和附件表的外键
    }
}
