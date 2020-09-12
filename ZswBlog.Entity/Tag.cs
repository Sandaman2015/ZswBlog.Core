using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 文章标签
    /// </summary>
    [Table("tag")]
    public partial class Tag
    {
        /// <summary>
        /// 文章标签Id
        /// </summary>
        [Display(Name = "文章标签Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagId { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        [Display(Name = "标签名称")]
        [Required()]
        [StringLength(50)]
        public string TagName { get; set; }
        /// <summary>
        /// 标签创建时间
        /// </summary>
        [Display(Name = "标签创建时间")]
        public DateTime TagCreateTime { get; set; }
    }
}
