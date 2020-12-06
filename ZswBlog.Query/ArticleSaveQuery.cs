using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.Query
{
    public class ArticleSaveQuery
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createDate { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 所属分类，默认为0是默认分类
        /// </summary>
        public int categoryId { get; set; }
        /// <summary>
        /// 是否显示1不显示,0显示
        /// </summary>
        public bool isShow { get; set; }
        /// <summary>
        /// 文章插图
        /// </summary>
        public string coverImage { get; set; }
        /// <summary>
        /// 是否置顶1不置顶,0置顶
        /// </summary>
        public bool isTop { get; set; }
        /// <summary>
        /// 置顶排序
        /// </summary>
        public int topSort { get; set; }

        /// <summary>
        /// 文章标签
        /// </summary>
        public List<int> tagIdList { get; set; }
    }
}
