using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        [FunctionDescription("获取所有已开放的友情链接")]
        public async Task<ActionResult<List<FriendLinkDTO>>> GetFriendLinks()
        {
            var friendLinkDtOs = await _friendLinkService.GetFriendLinksByIsShowAsync(true);
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
        public ActionResult SaveFriendLink([FromBody] FriendLinkEntity param)
        {
            param.src = System.Web.HttpUtility.HtmlEncode(param.src);
            param.portrait = System.Web.HttpUtility.HtmlEncode(param.portrait);
            param.description = System.Web.HttpUtility.HtmlEncode(param.description);
            param.title = System.Web.HttpUtility.HtmlEncode(param.title);
            param.createDate = DateTime.Now;
            param.isShow = false;
            var flag = _friendLinkService.AddEntity(param);
            return Ok(flag);
        }

        /// <summary>
        /// 后台管理-分页获取友情连接列表
        /// </summary>
        /// <param name="limit">页码</param>
        /// <param name="pageIndex">页数</param>
        /// <returns></returns>
        [Route("/api/friendlink/admin/get/page")]
        [Authorize]
        [HttpGet]
        [FunctionDescription("后台管理-分页获取友情连接列表")]
        public async Task<ActionResult<PageDTO<FriendLinkDTO>>> GetAnnouncementListByPage([FromQuery] int limit,
            [FromQuery] int pageIndex)
        {
            var friendLinkPage = await _friendLinkService.GetFriendLinkListByPageAsync(limit, pageIndex);
            return Ok(friendLinkPage);
        }

        /// <summary>
        /// 后台管理-友情连接更新
        /// </summary>
        /// <param name="entity">友情连接保存入参</param>
        /// <returns></returns>
        [Route("/api/friendlink/admin/update")]
        [Authorize]
        [HttpPost]
        [FunctionDescription("后台管理-友情连接更新")]
        public ActionResult<bool> UpdateFriendLink([FromBody] FriendLinkEntity entity)
        {
            var flag = _friendLinkService.UpdateFriendLink(entity);
            return Ok(flag);
        }

        /// <summary>
        /// 后台管理-友情连接删除
        /// </summary>
        /// <param name="id">友情连接主键</param>
        /// <returns></returns>
        [Route("/api/friendlink/admin/remove/{id}")]
        [Authorize]
        [HttpDelete]
        [FunctionDescription("后台管理-删除友情连接")]
        public ActionResult<bool> RemoveFriendLink([FromRoute] int id)
        {
            var flag = _friendLinkService.RemoveFriendLinkById(id);
            return Ok(flag);
        }

        /// <summary>
        /// 获取友情连接详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [Route("/api/friendlink/admin/get/{id}")]
        [Authorize]
        [HttpGet]
        [FunctionDescription("后台管理-获取友情连接详情")]
        public async Task<ActionResult<FriendLinkDTO>> GetFriendLinkById([FromRoute]int id)
        {
            var friendLink = await _friendLinkService.GetFriendLinkByIdAsync(id);
            return Ok(friendLink);
        }
    }
}
