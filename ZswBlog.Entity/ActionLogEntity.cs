using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 操作日志实体对象
    /// </summary>
    [Table("tab_actionlog")]
    public class ActionLogEntity : BaseEntity
    {
        /// <summary>
        /// 操作id
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
        public string operatorId { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string moduleName { get; set; }
        /// <summary>
        /// 操作详情
        /// </summary>
        public string actionDetail { get; set; }
        /// <summary>
        /// 操作地址
        /// </summary>
        public string actionUrl { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string ipAddress { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public int logType { get; set; }
    }
}
