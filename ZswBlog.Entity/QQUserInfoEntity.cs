using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.Entity
{
    /// <summary>
    /// QQ互联用户信息
    /// </summary>
    public class QQUserInfoEntity
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        public int id { get; set; }
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
        /// <summary>
        /// 40*40的头像
        /// </summary>
        public string figureurl_qq_1 { get; set; }
        /// <summary>
        /// QQ昵称
        /// </summary>
        public string nickName { get; set; }
        /// <summary>
        /// 绑定用户id
        /// </summary>
        public int userId { get; set; }
    }
}
