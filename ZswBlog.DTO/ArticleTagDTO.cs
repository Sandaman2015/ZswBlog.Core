using System;
using System.Collections.Generic;

namespace ZswBlog.DTO
{
    /// <summary>
    ///文章表标签
    /// </summary>
    public partial class ArticleTagDTO
    {
        /// <summary>
        /// 中间表id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createDate { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public int operatorId { get; set; }
        /// <summary>
        /// 文章id
        /// </summary>
        public int articleId { get; set; }
        /// <summary>
        /// 标签id
        /// </summary>
        public int tagId { get; set; }
    }
}
