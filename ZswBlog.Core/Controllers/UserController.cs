using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;
using ZswBlog.ThirdParty;

namespace ZswBlog.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IQQUserInfoService _userInfoService;

        private readonly IMapper _mapper;

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
        [Route("/user/get/near")]
        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetUserOnNearLogin()
        {
            List<UserDTO> userDTOs;
            //读取缓存
            userDTOs = await RedisHelper.GetAsync<List<UserDTO>>("ZswBlog:User:NearLoginUser");
            if (userDTOs == null)
            {
                //开启redis缓存
                userDTOs = _userService.GetUsersNearVisit(12);
                await RedisHelper.SetAsync("ZswBlog:User:NearLoginUser", userDTOs, 1200);
            }
            return Ok(userDTOs);
        }
        /// <summary>
        /// 保存邮箱
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("/user/save/email")]
        [HttpPost]
        public async Task<ActionResult<bool>> SaveEmail([FromBody] UserEntity user)
        {
            return await Task.Run(() =>
            {
                UserDTO userInfo = _userService.GetUserById(user.id);
                userInfo.email = user.email;
                user = _mapper.Map<UserEntity>(userInfo);
                bool flag = _userService.UpdateEntity(user);
                return Ok(flag);
            });
        }

        /// <summary>
        /// 获取QQ登录用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [Route("/user/login/qq")]
        [HttpGet]
        public async Task<ActionResult> QQLoginByAccessToken([FromQuery] string accessToken, string returnUrl)//分页面跳转可以多带一个参数
        {
            return await Task.Run(() =>
            {
                dynamic returnData;
                string jsonResult = "登录失败";
                UserDTO userDTO = _userInfoService.GetUserByAccessToken(accessToken);
                if (userDTO == null)
                {
                    jsonResult = "本次登录没有找到您的信息，不如刷新试试重新登录吧";
                    returnData = new { msg = jsonResult, url = returnUrl,code = 400 };
                }
                else
                {
                    UserDTO user = RedisHelper.Get<UserDTO>("ZswBlog:UserInfo:" + userDTO.id);
                    if (user == null)
                    {
                        RedisHelper.Set("ZswBlog:UserInfo:" + userDTO.id, userDTO, 60 * 60 * 6);
                    }
                    jsonResult = "登录成功！欢迎您：" + userDTO.nickName;
                    returnData = new { msg = jsonResult, code = 200, user = userDTO, userEmail = userDTO.email, url = returnUrl };
                }
                return Ok(returnData);
            });
        }

        /// <summary>
        /// 根据Token获取用户信息
        /// </summary>
        /// <returns></returns>
        [Route("/user/admin/get/info")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<dynamic>> GetUserInfoByAccessToken()
        {
            return await Task.Run(() =>
            {
                dynamic returnValue = new { url = "/admin/login", msg = "请重新登录！" };
                string bearer = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrEmpty(bearer) || !bearer.Contains("Bearer"))
                {
                    return returnValue;
                }
                string[] jwt = bearer.Split(' ');
                var tokenObj = new JwtSecurityToken(jwt[1]);

                var claimsIdentity = new ClaimsIdentity(tokenObj.Claims);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                int userId = int.Parse(claimsPrincipal.FindFirstValue("userId"));
                UserDTO userDTO = _userService.GetUserById(userId);
                return userDTO;
            });
        }
    }
}
