using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.DTO
{
    /// <summary>
    /// 文章数据传输对象
    /// </summary>
    public class ArticleDTO
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ArticleDTO()
        {
            this.tags = new List<TagDTO>();
        }
        /// <summary>
        /// 文章id
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
        /// 文章标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int like { get; set; }

        /// <summary>
        /// 所属分类，默认为0是默认分类
        /// </summary>
        [NotMapped]
        public int categoryId { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int visits { get; set; }

        /// <summary>
        /// 是否显示1不显示,0显示
        /// </summary>
        [NotMapped]
        public bool isShow { get; set; }

        /// <summary>
        /// 上次更新时间
        /// </summary>
        public DateTime lastUpdateDate { get; set; }

        /// <summary>
        /// 是否置顶1不置顶,0置顶
        /// </summary>
        public bool isTop { get; set; }

        /// <summary>
        /// 置顶排序
        /// </summary>
        public int topSort { get; set; }

        /// <summary>
        /// 阅读时间
        /// </summary>
        public int readTime { get; set; }

        /// <summary>
        /// 文章总字数
        /// </summary>
        public int textCount { get; set; }

        /// <summary>
        /// 文章分类对象
        /// </summary>
        [NotMapped]
        public CategoryDTO category { get; set; }

        /// <summary>
        /// 文章标签
        /// </summary>
        [NotMapped]
        public List<TagDTO> tags { get; set; }

        /// <summary>
        /// 文章背景图片
        /// </summary>
        public string coverImage { get;set;}
    }
}
