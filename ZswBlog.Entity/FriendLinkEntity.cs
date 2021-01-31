using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 友情链接实体对象
    /// </summary>
    [Table("tab_friendlink")]
    public class FriendLinkEntity : BaseEntity
    {
        /// <summary>
        /// 友情链接id
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
        /// 友情链接标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 友情连接头像地址
        /// </summary>
        public string portrait { get; set; }
        /// <summary>
        /// 友情链接地址
        /// </summary>
        public string src { get; set; }
        /// <summary>
        /// 友情链接描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 是否显示1不显示,0显示
        /// </summary>
        public bool isShow { get; set; }
    }
}
