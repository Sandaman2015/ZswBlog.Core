namespace ZswBlog.ThirdParty
{
    public class QQModel
    {
        /// <summary>
        /// 接口调用凭证
        /// </summary>
        public string Access_token { get; set; }
        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public string Expires_in { get; set; }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string Refresh_token { get; set; }
    }

    public class QQUser
    {
        /// <summary>
        /// OpenID是此网站上或应用中唯一对应用户身份的标识，网站或应用可将此ID进行存储，便于用户下次登录时辨识其身份，或将其与用户在网站上或应用中的原有账号进行绑定。
        /// </summary>
        public string Openid { get; set; }
    }

    public class QQUserInfo
    {
        /// <summary>
        /// 返回码，0: 正确返回，其它: 失败。
        /// </summary>
        public int Ret { get; set; }
        /// <summary>
        /// 如果ret小于0，会有相应的错误信息提示，返回数据全部用UTF-8编码。
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 用户在QQ空间的昵称。
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 大小为30×30像素的QQ空间头像URL。
        /// </summary>
        public string Figureurl { get; set; }
        /// <summary>
        /// 大小为50×50像素的QQ空间头像URL。
        /// </summary>
        public string Figureurl_1 { get; set; }
        /// <summary>
        /// 大小为100×100像素的QQ空间头像URL。
        /// </summary>
        public string Figureurl_2 { get; set; }
        /// <summary>
        /// 大小为40×40像素的QQ头像URL。
        /// </summary>
        public string Figureurl_qq_1 { get; set; }
        /// <summary>
        /// 大小为100×100像素的QQ头像URL。需要注意，不是所有的用户都拥有QQ的100x100的头像，但40x40像素则是一定会有。
        /// </summary>
        public string Figureurl_qq_2 { get; set; }
        /// <summary>
        /// 性别，如果获取不到则默认返回"男"。
        /// </summary>
        public string Gender { get; set; }
    }

}
