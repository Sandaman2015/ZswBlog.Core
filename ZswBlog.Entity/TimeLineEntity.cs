using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZswBlog.Entity.DbContext
{
    /// <summary>
    /// 时间线实体
    /// </summary>
    [Table("tab_timeline")]
    public class TimeLineEntity
    {
        /// <summary>
        /// 时间线id
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
        /// 时间线标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 时间线内容
        /// </summary>
        public string content { get; set; }
    }
}
