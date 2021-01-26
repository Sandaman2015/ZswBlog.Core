using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZswBlog.Common.Jwt;
using ZswBlog.Core.config;
using ZswBlog.Entity.DbContext;
using ZswBlog.IServices;
using ZswBlog.Query;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// Token分发
    /// </summary>
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="options"></param>
        public AuthorizeController(
            IUserService userService,
            IOptions<JwtSettings> options)
        {
            _userService = userService;
            _jwtSettings = options.Value;
        }

        /// <summary>
        ///  获取 token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("/api/authorize/get/token")]
        [HttpPost]
        [FunctionDescription("获取后台操作token")]
        public async Task<ActionResult<object>> GetToken([FromBody] UserVerifyQuery request)
        {
            dynamic respData;
            var isValidate = await _userService.ValidatePasswordAsync(request.username, request.password);
            if (isValidate == null)
            {
                respData = new {flag = false, msg = "用户名或密码错误"};
                return respData;
            }

            //可扩展自定义返回参数
            var claims = new[]
            {
                new Claim("userId", isValidate.id.ToString()),
                new Claim("userName", request.username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                DateTime.Now,
                DateTime.Now.AddSeconds(1800),
                creds);
            //获取JWT生成的Token
            respData = new
            {
                flag = true, express = DateTime.Now.AddSeconds(1800),
                accessToken = new JwtSecurityTokenHandler().WriteToken(token)
            };
            return Ok(respData);
        }
    }
}