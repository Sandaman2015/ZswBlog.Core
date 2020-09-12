using System.Collections.Generic;

namespace ZswBlog.DTO
{
    /// <summary>
    ///文章表标签
    /// </summary>
    public partial class ArticleTagDTO
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 文章列表
        /// </summary>
        public List<MiniArticleDTO> ArticleList { get; set; }
    }
}
