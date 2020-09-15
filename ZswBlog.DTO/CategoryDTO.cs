using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.DTO
{
    /// <summary>
    /// 文章分类表
    /// </summary>
    public class CategoryDTO
    {
        /// <summary>
        /// 分类主键
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public int operatorId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createDate { get; set; }
        /// <summary>
        /// 分类描述
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 文章数量
        /// </summary>
        public int articleCount { get; set; }
    }
}
