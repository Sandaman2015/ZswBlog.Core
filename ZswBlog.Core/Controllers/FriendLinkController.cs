using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IFriendLinkService _friendLinkSerivce;

        public FriendLinkController(IFriendLinkService friendLinkSerivce)
        {
            _friendLinkSerivce = friendLinkSerivce;
        }

        /// <summary>
        /// 获取所有友情链接
        /// </summary>
        /// <returns></returns>
        [Route("/api/friendlink/get/all")]
        [HttpGet]
        public ActionResult<List<FriendLinkDTO>> GetFriendLinks()
        {
            List<FriendLinkDTO> friendLinkDTOs = _friendLinkSerivce.GetFriendLinksByIsShow(true);
            //读取缓存
            //friendLinkDTOs = await RedisHelper.GetAsync<List<FriendLinkDTO>>("ZswBlog:FriendLink:FriendLinkDTOList");
            //if (friendLinkDTOs == null)
            //{
            // 开启缓存
                //await RedisHelper.SetAsync("ZswBlog:FriendLink:FriendLinkList", friendLinkDTOs, 1200);
            //}
            return Ok(friendLinkDTOs);
        }

        /// <summary>
        /// 申请友情链接
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Route("/api/friendlink/save")]
        [HttpPost]
        public async Task<ActionResult> SaveFriendLink([FromBody]FriendLinkEntity param)
        {
            return await Task.Run(() =>
            {
                param.src = System.Web.HttpUtility.HtmlEncode(param.src);
                param.portrait = System.Web.HttpUtility.HtmlEncode(param.portrait);
                param.description = System.Web.HttpUtility.HtmlEncode(param.description);
                param.title = System.Web.HttpUtility.HtmlEncode(param.title);
                param.createDate = DateTime.Now;
                param.isShow = false;
                bool flag = _friendLinkSerivce.AddEntity(param);
                return Ok(flag);
            });            
        }
    }
}
