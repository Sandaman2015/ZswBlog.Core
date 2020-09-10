using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IServices;
using ZswBlog.ThirdParty;

namespace ZswBlog.Web.Controllers
{
    /// <summary>
    /// 登录
    /// </summary>
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService userService;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“LoginController.LoginController(IUserService)”的 XML 注释
        public LoginController(IUserService userService)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“LoginController.LoginController(IUserService)”的 XML 注释
        {
            this.userService = userService;
        }
        /// <summary>
        /// 保存邮箱
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddEmail(User user)
        {
            int Code;
            User user1 = await userService.GetUserByIdAsync(user.UserId);
            user1.UserEmail = user.UserEmail;
            Code = await userService.AlterEntityAsync(user1) ? 200 : 500;
            return Ok(new { code = Code });
        }
        /// <summary>
        /// 获取QQ登录用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetQQUserInfo([FromQuery] string accessToken)//分页面跳转可以多带一个参数
        {
            string jsonResult = "";
            Guid InfoId = Guid.Parse("00000000-0000-0000-0000-000000000000");
            string email = "";
            QQUserInfo qqUserInfo = new QQUserInfo();
            User user = new User();
            QQLogin login = new QQLogin();
            string openId = login.GetOpenID(accessToken);
            qqUserInfo = login.GetQQUserInfo(accessToken, openId);
            if (qqUserInfo.Ret == 0 && string.IsNullOrWhiteSpace(qqUserInfo.Msg)) //成功
            {
                user.UserPortrait = qqUserInfo.Figureurl_qq_1;
                user.UserName = qqUserInfo.Nickname;
                user.UserAccessToken = accessToken;
                user.UserOpenId = openId;
                user.UserId = new Guid();
                if (await userService.AddEntityAsync(user))
                {
                    var userInfo = await userService.GetUserByOpenIdAsync(user.UserOpenId);
                    InfoId = userInfo.UserId;
                    email = userInfo.UserEmail;
                    jsonResult = "登录成功！欢迎您：" + qqUserInfo.Nickname;
                }
                else
                {
                    jsonResult = "登录成功！但是后台出了点问题,刷新重新试试吧！";
                }
            }
            else //失败
            {
                jsonResult = "登录失败！返回码：" + qqUserInfo.Ret;
            }
            return Ok(new { msg = jsonResult, data = qqUserInfo, userId = InfoId, userEmail = email });
        }

    }
}
