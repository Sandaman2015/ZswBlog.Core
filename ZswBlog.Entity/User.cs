using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    [Table("frameworkusers")]
    public class User
    {
        [Key]
        [Column("ID")]
        public Guid UserId { get; set; }
        [Display(Name = "用户密码")]
        [Column("Password")]
        public string UserPassword { get; set; } = "123456";
        [Display(Name = "用户姓名")]
        [Column("Name")]
        public string UserName { get; set; }
        [Display(Name = "用户邮箱")]
        [Column("Email")]
        public string UserEmail { get; set; }
        [Display(Name = "账户")]
        public string ITCode { get; set; }//可以使用QQ名称
        [Display(Name = "用户验证Token")]
        [StringLength(200)]
        public string UserAccessToken { get; set; }
        [Display(Name = "用户登陆时间")]
        public DateTime UserLoginTime { get; set; }
        [Display(Name = "用户头像")]
        [StringLength(400)]
        public string UserPortrait { get; set; }
        [Display(Name = "用户创建时间")]
        public DateTime UserCreateTime { get; set; }
        [Display(Name = "用户上次登录时间")]
        public DateTime UserLastLoginTime { get; set; }
        [Display(Name = "用户登录次数")]
        public int UserLoginCount { get; set; }
        [Display(Name = "用户的第三方OpenId")]
        [StringLength(200)]
        public string UserOpenId { get; set; }
        public string Discriminator { get; set; } = "User";
        public bool IsValid { get; set; } = false;
    }
}
