using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZswBlog.Entity.DbContext
{
    /// <summary>
    /// 文章标签表
    /// </summary>
    [Table("tab_middle_article_tag")]
    public class ArticleTagEntity
    {
        /// <summary>
        /// 中间表id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        /// 关联外键多对多
        /// </summary>
        [ForeignKey("articleId")]
        public virtual ArticleEntity article { get; set; }

        /// <summary>
        /// 标签id
        /// </summary>
        public int tagId { get; set; }
        
        /// <summary>
        /// 关联外键多对多
        /// </summary>
        [ForeignKey("tagId")]
        public virtual TagEntity tag { get; set; }
    }
}
