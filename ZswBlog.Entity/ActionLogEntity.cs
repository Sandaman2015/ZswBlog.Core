using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 操作日志实体对象
    /// </summary>
    public class ActionLogEntity
    {
        /// <summary>
        /// 操作id
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
        /// 模块名称
        /// </summary>
        public string moduleName { get; set; }
        /// <summary>
        /// 方法名称
        /// </summary>
        public string actionName { get; set; }
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
        public string logType { get; set; }
    }
}
