using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 标签实体对象
    /// </summary>
    public class TagEntity
    {
        /// <summary>
        /// 标签id
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
        /// 标签名称
        /// </summary>
        public string name { get; set; }
    }
}
