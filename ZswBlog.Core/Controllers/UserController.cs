using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NETCore.Encrypt;
using ZswBlog.Common;
using ZswBlog.Common.Util;
using ZswBlog.Core.config;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;
using ZswBlog.ThirdParty;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IQQUserInfoService _userInfoService;

        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        private static QQLogin qQLogin = new QQLogin();
        private static readonly ILogger Logger = LoggerFactory.Create(build =>
        {
            build.AddConsole(); // 用于控制台程序的输出
            build.AddDebug(); // 用于VS调试，输出窗口的输出
        }).CreateLogger("UserController");

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="mapper"></param>
        /// <param name="userInfoService"></param>
        /// <param name="configuration">地址</param>
        public UserController(IUserService userService, IMapper mapper, IQQUserInfoService userInfoService, IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _userInfoService = userInfoService;
            _configuration = configuration;
        }

        /// <summary>
        /// 获取最近登录用户
        /// </summary>
        /// <returns></returns>
        [Route("/api/user/get/near")]
        [HttpGet]
        [FunctionDescription("获取最近登录QQ用户")]
        public async Task<ActionResult<List<UserDTO>>> GetUserOnNearLogin()
        {
            //读取缓存
            var userDtOs = await RedisHelper.GetAsync<List<UserDTO>>("ZswBlog:User:NearLoginUser");
            if (userDtOs != null) return Ok(userDtOs);
            //开启redis缓存
            userDtOs = await _userService.GetUsersNearVisitAsync(12);
            await RedisHelper.SetAsync("ZswBlog:User:NearLoginUser", userDtOs, 1200);
            return Ok(userDtOs);
        }

        /// <summary>
        /// 保存邮箱
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("/api/user/save/email")]
        [HttpPost]
        [FunctionDescription("保存邮箱")]
        public async Task<ActionResult<bool>> SaveEmail([FromBody] UserEntity user)
        {
            var userInfo = await _userService.GetUserByIdAsync(user.id);
            userInfo.email = user.email;
            user = _mapper.Map<UserEntity>(userInfo);
            var flag = _userService.UpdateEntity(user);
            return Ok(flag);
        }

        /// <summary>
        /// 后台管理-分页获取登陆人员列表
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页码</param>
        /// <param name="nickName">模糊昵称</param>
        /// <param name="disabled">禁用</param>
        /// <returns></returns>
        [Route("/api/user/admin/get/page")]
        [HttpGet]
        [Authorize]
        [FunctionDescription("后台管理-分页获取登陆人员列表")]
        public async Task<ActionResult<PageDTO<UserDTO>>> GetUserListByPage([FromQuery] int pageIndex,
            [FromQuery] int pageSize, string nickName, bool disabled)
        {
            var pageList = await _userService.GetUserListByPage(pageIndex, pageSize, nickName, disabled);
            return Ok(pageList);
        }

        /// <summary>
        /// 获取登录的地址
        /// </summary>
        /// <returns></returns>
        [Route("/api/user/generate/qqurl")]
        [HttpGet]
        [FunctionDescription("QQ登录获取token")]
        public Dictionary<string, object> GenerateQQLoginUrl(string requestPath)
        {
            string generateUrl = qQLogin.GetAuthCodeUrl(out string state);
            //存储关系到redis中
            RedisHelper.Set("QQLogin:LoginKey:" + state, requestPath, 600);
            RedisHelper.Set("QQLogin:RequestFlag:" + state, true, 600);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("state", state);
            dict.Add("generateUrl", generateUrl);
            dict.Add("requestFlag", true);
            dict.Add("expireIn", DateTime.Now.AddMinutes(8));
            return dict;
        }

        /// <summary>
        /// QQ授权登录回调
        /// </summary>
        /// <returns></returns>
        [Route("api/front/user/login/qq/callback")]
        [HttpGet]
        [FunctionDescription("QQ授权登录回调")]
        public async Task<ActionResult> QQLoginCallBack([FromQuery] string state, [FromQuery] string code, [FromQuery] string redirect)
        {
            string requestPath = await RedisHelper.GetAsync("QQLogin:LoginKey:" + state);
            string url = qQLogin.GetAccessTokenUrl(code, requestPath);
            string accessToken = await qQLogin.GetAccessToken(url);
            UserDTO userDto = await _userInfoService.GetUserByAccessTokenAsync(accessToken);
            await RedisHelper.SetAsync("QQLogin:LoginUserInfo:" + state, userDto, 60 * 24 * 7);
            //存储关系到redis中
            redirect = await RedisHelper.GetAsync("QQLogin:LoginKey:" + state);
            return Redirect(redirect);
        }

        /// <summary>
        /// 轮训获取QQ登录用户信息
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [Route("/api/user/login/qq")]
        [HttpGet]
        [FunctionDescription("获取QQ登录用户信息")]
        public async Task<ActionResult> QQLoginCheck([FromQuery] string state)
        {
            dynamic returnData = new { code = 200 };
            bool userInfoGetFlag = true;
            while (userInfoGetFlag)
            {
                bool loginRequestFlag = await RedisHelper.GetAsync<bool>("QQLogin:RequestFlag:" + state);
                if (loginRequestFlag)
                {
                    UserDTO userDto = await RedisHelper.GetAsync<UserDTO>("QQLogin:LoginUserInfo:" + state);
                    if (userDto != null)
                    {
                        var jsonResult = "登录成功！欢迎您：" + userDto.nickName;
                        returnData = new { msg = jsonResult, code = 200, user = userDto, userEmail = userDto.email };
                        userInfoGetFlag = false;
                    }
                }else
                {
                    var jsonResult = "登录超时，请重试！";
                    //尝试获取一次用户信息，如果有则代表用户并未退出
                    UserDTO userDto = await RedisHelper.GetAsync<UserDTO>("QQLogin:LoginUserInfo:" + state);
                    if (userDto != null)
                    {
                        jsonResult = "登录成功！欢迎您：" + userDto.nickName;
                        returnData = new { msg = jsonResult, code = 200, user = userDto, userEmail = userDto.email };
                        return Ok(returnData);
                    }
                    returnData = new { msg = jsonResult, code = 200 };
                }
            }
            return Ok(returnData);
        }

        /// <summary>
        /// 根据Token获取用户信息
        /// </summary>
        /// <returns></returns>
        [Route("/api/user/admin/get/info")]
        [Authorize]
        [HttpGet]
        [FunctionDescription("根据QQ的Token获取QQ用户信息")]
        public async Task<ActionResult<dynamic>> GetUserInfoByAccessToken()
        {
            dynamic returnValue = new { url = "/admin/login", msg = "请重新登录！" };
            var bearer = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(bearer) || !bearer.Contains("Bearer"))
            {
                return returnValue;
            }

            var jwt = bearer.Split(' ');
            var tokenObj = new JwtSecurityToken(jwt[1]);
            var claimsIdentity = new ClaimsIdentity(tokenObj.Claims);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var userId = int.Parse(claimsPrincipal.FindFirstValue("userId"));
            var userDto = await _userService.GetUserByIdAsync(userId);
            return userDto;
        }
    }
}