using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    [Table("tag")]
    public partial class Tag
    {
        [Display(Name = "文章标签Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagId { get; set; }
        [Display(Name = "标签名称")]
        [Required()]
        [StringLength(50)]
        public string TagName { get; set; }
        [Display(Name = "标签创建时间")]
        public DateTime TagCreateTime { get; set; }
    }
}
