using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZswBlog.Entity.DbContext
{
    /// <summary>
    /// 标签实体对象
    /// </summary>
    [Table("tab_tag")]
    public class TagEntity
    {
        /// <summary>
        /// 标签id
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
        /// 标签名称
        /// </summary>
        public string name { get; set; }
    }
}
