using System;

namespace ZswBlog.DTO
{
    /// <summary>
    /// 文章
    /// </summary>
    public class ArticleDTO
    {
        /// <summary>
        /// 文章id
        /// </summary>
        public int ArticleId { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string ArticleTitle { get; set; }
        /// <summary>
        /// 文章分类
        /// </summary>
        public string ArticleClass { get; set; }
        /// <summary>
        /// 文章浏览人数
        /// </summary>
        public int ArticleVisits { get; set; }
        /// <summary>
        /// 文章喜爱数
        /// </summary>
        public int ArticleLikes { get; set; }
        /// <summary>
        /// 文章创建时间
        /// </summary>
        public DateTime ArticleTime { get; set; }
        /// <summary>
        /// 文章字数
        /// </summary>
        public int ArticleTextCount { get; set; }
        /// <summary>
        /// 文章阅读时间
        /// </summary>
        public int ArticleReadTime { get; set; }
        /// <summary>
        /// 文章创建
        /// </summary>
        public string ArticleCreatedBy { get; set; }
        /// <summary>
        /// 文章标签
        /// </summary>
        public string[] ArticleTags { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string ArticleContent { get; set; }
        /// <summary>
        /// 文章配图
        /// </summary>
        public string ArticleImage { get; set; }
    }
}
