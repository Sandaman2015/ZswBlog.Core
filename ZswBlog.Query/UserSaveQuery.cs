using System;

namespace ZswBlog.Query
{
    public class UserSaveQuery
    {
        /// <summary>
        /// 邮件地址
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string portrait { get; set; }
        /// <summary>
        /// QQ开放id
        /// </summary>
        public string openId { get; set; }
        /// <summary>
        /// accesstoken
        /// </summary>
        public string accessToken { get; set; }
        /// <summary>
        /// 性别：默认男
        /// </summary>
        public string gender { get; set; }
    }
}
