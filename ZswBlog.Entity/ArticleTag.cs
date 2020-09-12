using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 文章标签
    /// </summary>
    [Table("articletag")]
    public partial class ArticleTag
    {
        /// <summary>
        /// 文章标签Id
        /// </summary>
        [Display(Name = "文章标签Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AtId { get; set; }
        /// <summary>
        /// 文章实体
        /// </summary>
        public Article Article { get; set; }
        /// <summary>
        /// 文章Id
        /// </summary>
        [Display(Name = "文章Id")]
        public int ArticleId { get; set; }
        /// <summary>
        /// 标签实体
        /// </summary>
        public Tag Tag { get; set; }
        /// <summary>
        /// 标签Id
        /// </summary>
        [Display(Name = "标签Id")]
        public int TagId { get; set; }
    }
}
