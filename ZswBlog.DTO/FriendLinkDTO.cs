namespace ZswBlog.DTO
{
    /// <summary>
    /// 友情链接DTO
    /// </summary>
    public partial class FriendLinkDTO
    {
        /// <summary>
        /// 友情链接标题
        /// </summary>
        public string LinkTitle { get; set; }
        /// <summary>
        /// 友情链接图标
        /// </summary>
        public string LinkImage { get; set; }
        /// <summary>
        /// 友情链接地址
        /// </summary>
        public string LinkSrc { get; set; }
        /// <summary>
        /// 友情链接介绍
        /// </summary>
        public string LinkIntroduce { get; set; }
    }
}
