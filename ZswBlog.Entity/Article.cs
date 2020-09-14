using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 文章表
    /// </summary>
    [Table("article")]
    public partial class Article
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        [Display(Name = "文章Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticleId { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        [Display(Name = "文章标题")]
        [StringLength(50)]
        public string ArticleTitle { get; set; }
        /// <summary>
        /// 文章类型
        /// </summary>
        [Display(Name = "文章类型")]
        [StringLength(30)]
        public string ArticleClass { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        [Display(Name = "文章内容")]
        public string ArticleContent { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "文章创建时间")]
        public DateTime ArticleCreateTime { get; set; }
        /// <summary>
        /// 文章满意度
        /// </summary>
        [Display(Name = "文章满意度")]
        public int ArticleLikes { get; set; } = 0;
        /// <summary>
        /// 文章浏览次数
        /// </summary>
        [Display(Name = "文章浏览次数")]
        public int ArticleVisits { get; set; } = 0;
        /// <summary>
        /// 文章是否显示
        /// </summary>
        [Display(Name = "文章是否显示")]
        public bool IsShow { get; set; } = true;
        /// <summary>
        /// 文章上次更新时间
        /// </summary>
        [Display(Name = "文章上次更新时间")]
        public DateTime? ArticleLastUpdate { get; set; }
        /// <summary>
        /// 文章插图
        /// </summary>
        [Display(Name = "文章插图")]
        [Column("ArticleImageID")]
        public Guid ArticleImage { get; set; }


    }
}
