using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    [Table("articletag")]
    public partial class ArticleTag
    {
        [Display(Name = "文章标签Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AtId { get; set; }
        public Article Article { get; set; }
        [Display(Name = "文章Id")]
        public int ArticleId { get; set; }
        public Tag Tag { get; set; }
        [Display(Name = "标签Id")]
        public int TagId { get; set; }
    }
}
