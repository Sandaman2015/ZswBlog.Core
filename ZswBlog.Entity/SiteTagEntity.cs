using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 站点标签实体对象
    /// </summary>
    [Table("tab_sitetag")]
    public class SiteTagEntity : BaseEntity
    {
        /// <summary>
        /// 站点标签id
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
        /// 站点标签标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int like { get; set; }
        /// <summary>
        /// 是否显示1不显示,0显示
        /// </summary>
        public bool isShow { get; set; }
    }
}
