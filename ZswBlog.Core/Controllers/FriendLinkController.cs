using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.Common;
using ZswBlog.Core.config;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 友情链接
    /// </summary>
    [ApiController]
    public class FriendLinkController : ControllerBase
    {
        private readonly IFriendLinkService _friendLinkService;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="friendLinkService"></param>
        public FriendLinkController(IFriendLinkService friendLinkService)
        {
            _friendLinkService = friendLinkService;
        }

        /// <summary>
        /// 获取所有友情链接
        /// </summary>
        /// <returns></returns>
        [Route("/api/friendLink/get/all")]
        [HttpGet]
        [FunctionDescription("获取所有友情链接")]
        public async Task<ActionResult<List<FriendLinkDTO>>> GetFriendLinks()
        {
            var friendLinkDtOs = await  _friendLinkService.GetFriendLinksByIsShowAsync(true);
            //读取缓存
            //friendLinkDTOs = await RedisHelper.GetAsync<List<FriendLinkDTO>>("ZswBlog:FriendLink:FriendLinkDTOList");
            //if (friendLinkDTOs == null)
            //{
            // 开启缓存
                //await RedisHelper.SetAsync("ZswBlog:FriendLink:FriendLinkList", friendLinkDTOs, 1200);
            //}
            return Ok(friendLinkDtOs);
        }

        /// <summary>
        /// 申请友情链接
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Route("/api/friendlink/save")]
        [HttpPost]
        [FunctionDescription("申请友情链接")]
        public async Task<ActionResult> SaveFriendLink([FromBody]FriendLinkEntity param)
        {
                param.src = System.Web.HttpUtility.HtmlEncode(param.src);
                param.portrait = System.Web.HttpUtility.HtmlEncode(param.portrait);
                param.description = System.Web.HttpUtility.HtmlEncode(param.description);
                param.title = System.Web.HttpUtility.HtmlEncode(param.title);
                param.createDate = DateTime.Now;
                param.isShow = false;
                var flag = await _friendLinkService.AddEntityAsync(param);
                return Ok(flag);
        }
    }
}
