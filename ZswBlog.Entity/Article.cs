using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    [Table("article")]
    public partial class Article
    {
        [Display(Name = "文章Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticleId { get; set; }
        [Display(Name = "文章标题")]
        [StringLength(50)]
        public string ArticleTitle { get; set; }
        [Display(Name = "文章类型")]
        [StringLength(30)]
        public string ArticleClass { get; set; }
        [Display(Name = "文章内容")]
        public string ArticleContent { get; set; }
        [Display(Name = "文章创建时间")]
        public DateTime ArticleCreateTime { get; set; }
        [Display(Name = "文章满意度")]
        public int ArticleLikes { get; set; } = 0;
        [Display(Name = "文章浏览次数")]
        public int ArticleVisits { get; set; } = 0;
        [Display(Name = "文章是否显示")]
        public int IsShow { get; set; } = 0;
        [Display(Name = "文章上次更新时间")]
        public DateTime? ArticleLastUpdate { get; set; }
        [Display(Name = "文章插图")]
        [Column("ArticleImageID")]
        public Guid ArticleImage { get; set; }
    }
}
