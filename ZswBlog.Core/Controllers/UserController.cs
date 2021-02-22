using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.Common;
using ZswBlog.Core.config;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;

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

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="mapper"></param>
        /// <param name="userInfoService"></param>
        public UserController(IUserService userService, IMapper mapper, IQQUserInfoService userInfoService)
        {
            _userService = userService;
            _mapper = mapper;
            _userInfoService = userInfoService;
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
            var flag = await _userService.UpdateEntityAsync(user);
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
        /// 获取QQ登录用户信息
        /// </summary>
        /// <param name="accessToken">QQ的Token</param>
        /// <param name="returnUrl">分页面跳转可以多带一个参数</param>
        /// <returns></returns>
        [Route("/api/user/login/qq")]
        [HttpGet]
        [FunctionDescription("获取QQ登录用户信息")]
        public async Task<ActionResult> QqLoginByAccessToken([FromQuery] string accessToken, string returnUrl)
        {
            dynamic returnData;
            var jsonResult = "登录失败";
            var userDto = await _userInfoService.GetUserByAccessTokenAsync(accessToken);
            if (userDto == null)
            {
                jsonResult = "本次登录没有找到您的信息，不如刷新试试重新登录吧";
                returnData = new {msg = jsonResult, url = returnUrl, code = 400};
            }
            else
            {
                var user = await RedisHelper.GetAsync<UserDTO>("ZswBlog:UserInfo:" + userDto.id);
                if (user == null)
                {
                    RedisHelper.SetAsync("ZswBlog:UserInfo:" + userDto.id, userDto, 60 * 60 * 6);
                }

                jsonResult = "登录成功！欢迎您：" + userDto.nickName;
                returnData = new
                    {msg = jsonResult, code = 200, user = userDto, userEmail = userDto.email, url = returnUrl};
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
            dynamic returnValue = new {url = "/admin/login", msg = "请重新登录！"};
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