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
        public Guid UserId { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserPortrait { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
    }
}
