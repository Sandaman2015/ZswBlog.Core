using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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
        /// <returns></returns>
        [Route("/user/login/QQ")]
        [HttpGet]
        public async Task<ActionResult> QQLoginByAccessToken([FromQuery] string accessToken)//分页面跳转可以多带一个参数
        {
            return await Task.Run(() =>
            {
                dynamic returnData;
                string jsonResult = "登录失败";
                UserDTO userDTO = _userService.GetUserByAccessToken(accessToken);
                if (userDTO == null)
                {
                    jsonResult = "本次登录没有找到您的信息，不如刷新试试重新登录吧";
                    returnData = new { msg = jsonResult };
                }
                else
                {
                    UserDTO user = RedisHelper.Get<UserDTO>("ZswBlog:UserInfo:" + userDTO.id);
                    if (user == null)
                    {
                        RedisHelper.Set("ZswBlog:UserInfo:" + userDTO.id, userDTO, 60 * 60 * 6);
                    }
                    jsonResult = "登录成功！欢迎您：" + userDTO.nickName;
                    returnData = new { msg = jsonResult, userId = userDTO, userEmail = userDTO.email };
                }
                return Ok(returnData);
            });
        }
    }
}
