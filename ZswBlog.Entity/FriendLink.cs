using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 友情连接
    /// </summary>
    [Table("friendlink")]
    public partial class FriendLink
    {
        /// <summary>
        /// 友链Id
        /// </summary>
        [Display(Name = "友链Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FriendlinkId { get; set; }
        /// <summary>
        /// 友链标题
        /// </summary>
        [Display(Name = "友链标题")]
        [Required()]
        [StringLength(50)]
        public string FriendlinkTitle { get; set; }
        /// <summary>
        /// 友链创建时间
        /// </summary>
        [Display(Name = "友链创建时间")]
        [Required()]
        public DateTime FriendlinkCreateTime { get; set; }
        /// <summary>
        /// 友链的图片
        /// </summary>
        [Display(Name = "友链头像")]
        [StringLength(350)]
        public string FriendlinkImage { get; set; }
        /// <summary>
        /// 友链链接
        /// </summary>
        [Display(Name = "友链链接")]
        [StringLength(100)]
        public string Friendlink { get; set; }
        /// <summary>
        /// 是否显示友链
        /// </summary>
        [Display(Name = "是否显示友链")]
        [DefaultValue(0)]
        public bool IsShow { get; set; } = false;
        /// <summary>
        /// 友链介绍
        /// </summary>
        [Display(Name = "友链介绍")]
        [StringLength(80)]
        public string FriendlinkIntroduce { get; set; }
    }
}
