using System;

namespace ZswBlog.DTO
{
    /// <summary>
    /// 用户对象
    /// </summary>
    public partial class UserDTO
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 邮件地址
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickName { get; set; }
        /// <summary>
        /// 首次登录
        /// </summary>
        public string loginTime { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string portrait { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createDate { get; set; }
        /// <summary>
        /// 上次登录
        /// </summary>
        public DateTime lastLoginDate { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public int loginCount { get; set; }
    }
}
