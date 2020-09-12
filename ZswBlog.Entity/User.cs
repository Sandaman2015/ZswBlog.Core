using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("frameworkusers")]
    public class User
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Key]
        [Column("ID")]
        public Guid UserId { get; set; }

        //TODO 该密码可以删除
        /// <summary>
        /// 用户密码:默认12346
        /// </summary>
        [Display(Name = "用户密码")]
        [Column("Password")]
        public string UserPassword { get; set; } = "123456";
        /// <summary>
        /// 用户姓名
        /// </summary>
        [Display(Name = "用户昵称")]
        [Column("Name")]
        public string UserName { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        [Display(Name = "用户邮箱")]
        [Column("Email")]
        public string UserEmail { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        [Display(Name = "账户")]
        public string ITCode { get; set; }//可以使用QQ名称
        /// <summary>
        /// 第三方Token
        /// </summary>
        [Display(Name = "用户验证Token")]
        [StringLength(200)]
        public string UserAccessToken { get; set; }
        /// <summary>
        /// 用户登陆时间
        /// </summary>
        [Display(Name = "用户登陆时间")]
        public DateTime UserLoginTime { get; set; }
        /// <summary>
        /// 用户QQ头像默认16*16
        /// </summary>
        [Display(Name = "用户头像")]
        [StringLength(400)]
        public string UserPortrait { get; set; }
        /// <summary>
        /// 用户创建时间
        /// </summary>
        [Display(Name = "用户创建时间")]
        public DateTime UserCreateTime { get; set; }
        /// <summary>
        /// 用户上次登录时间
        /// </summary>
        [Display(Name = "用户上次登录时间")]
        public DateTime UserLastLoginTime { get; set; }
        /// <summary>
        /// 用户登录次数
        /// </summary>
        [Display(Name = "用户登录次数")]
        public int UserLoginCount { get; set; }
        /// <summary>
        /// 用户的第三方OpenId
        /// </summary>
        [Display(Name = "用户的第三方OpenId")]
        [StringLength(200)]
        public string UserOpenId { get; set; }
        /// <summary>
        /// 辨别者
        /// </summary>
        public string Discriminator { get; set; } = "User";
        /// <summary>
        /// 验证
        /// </summary>
        public bool IsValid { get; set; } = false;
    }
}
