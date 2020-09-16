using System;

namespace ZswBlog.DTO
{
    /// <summary>
    /// 友情链接DTO
    /// </summary>
    public partial class FriendLinkDTO
    {
        /// <summary>
        /// 友情链接id
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
