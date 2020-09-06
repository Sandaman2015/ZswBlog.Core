using System.Collections.Generic;

namespace ZswBlog.DTO
{
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
