using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZswBlog.Entity.DbContext
{
    /// <summary>
    /// 分类实体对象
    /// </summary>
    [Table("tab_category")]
    public class CategoryEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public CategoryEntity()
        {
            this.articles = new List<ArticleEntity>();
        }

        /// <summary>
        /// 分类主键
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        /// 多对多模型
        /// </summary>
        public virtual List<ArticleEntity> articles { get; set; }
    }
}