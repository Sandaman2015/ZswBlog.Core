using System.Collections.Generic;

namespace ZswBlog.DTO
{
    /// <summary>
    /// 页面初始化数据
    /// </summary>
    public class IndexInitDataDTO
    {
        /// <summary>
        /// 留言列表前二十
        /// </summary>
        public List<MessageDTO> Messages { get; set; }
        /// <summary>
        /// 文章列表前三
        /// </summary>
        public List<ArticleDTO> Articles { get; set; }
        /// <summary>
        /// 统计数据
        /// </summary>
        public CountData DataCount { get; set; }
    }
    /// <summary>
    /// 页面统计数据
    /// </summary>
    public class CountData
    {
        /// <summary>
        /// 页面浏览数
        /// </summary>
        public int VisitsCount { get; set; }
        /// <summary>
        /// 文章总数
        /// </summary>
        public int ArticleCount { get; set; }
        /// <summary>
        /// 网站运行时间
        /// </summary>
        public int RunDays { get; set; }
        /// <summary>
        /// 标签总数
        /// </summary>
        public int TagsCount { get; set; }
    }
}
