namespace ZswBlog.ThirdParty
{
    public interface IOAuthConfig
    {
        /// <summary>
        /// 请求的基本网址
        /// 如:https://api.weibo.com
        /// </summary>
        string BaseUrl { get; }

        /// <summary>
        /// AppKey
        /// </summary>
        string AppKey { get; }

        /// <summary>
        /// AppSecret
        /// </summary>
        string AppSecret { get; }

        /// <summary>
        /// 回调域名
        /// </summary>
        string Domain { get; }
    }

}
