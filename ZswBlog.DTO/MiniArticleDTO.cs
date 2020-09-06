using System;

namespace ZswBlog.DTO
{
    /// <summary>
    /// 文章简略对象
    /// </summary>
    public partial class MiniArticleDTO
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
        /// 文章时间
        /// </summary>
        public DateTime ArticleTime { get; set; }
        /// <summary>
        /// 文章内容（简述）
        /// </summary>
        public string ArticleContent { get; set; }
    }
}
