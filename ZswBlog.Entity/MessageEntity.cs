using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZswBlog.Entity.DbContext
{
    /// <summary>
    /// 留言实体对象
    /// </summary>
    [Table("tab_message")]
    public class MessageEntity
    {
        /// <summary>
        /// 留言id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 留言时间
        /// </summary>
        public DateTime createDate { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 目标留言用户
        /// </summary>
        public int? targetUserId { get; set; }
        /// <summary>
        /// 目标留言id
        /// </summary>
        public int? targetId { get; set; }
        /// <summary>
        /// 留言位置
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>
        public string browser { get; set; }
    }
}
