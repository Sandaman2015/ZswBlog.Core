using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 旅行实体对象
    /// </summary>
    [Table("tab_travel")]
    public class TravelEntity : BaseEntity
    {
        /// <summary>
        /// 旅行分享id
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
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 旅行分享内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 是否显示1不显示,0显示
        /// </summary>
        public bool isShow { get; set; }

        /// <summary>
        /// 通过创建
        /// </summary>
        public string createBy{get;set;}

        /// <summary>
        /// 多对多外键
        /// </summary>
        public virtual List<TravelFileAttachmentEntity> imgList { get; set; }
    }
}
