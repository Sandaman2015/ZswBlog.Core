using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;
using ZswBlog.MapperFactory;

namespace ZswBlog.Web.Controllers
{
    /// <summary>
    /// 友链页
    /// </summary>
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly IFriendLinkService friendLinkSerivce;
        private readonly FriendLinkMapper friendLinkMapper;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“LinkController.LinkController(IFriendLinkService, FriendLinkMapper)”的 XML 注释
        public LinkController(IFriendLinkService friendLinkSerivce, FriendLinkMapper friendLinkMapper)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“LinkController.LinkController(IFriendLinkService, FriendLinkMapper)”的 XML 注释
        {
            this.friendLinkSerivce = friendLinkSerivce;
            this.friendLinkMapper = friendLinkMapper;
        }
        /// <summary>
        /// 获取所有友情链接
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FriendLinkDTO>>> GetFriendLinks()
        {
            IEnumerable<FriendLinkDTO> friendLinkDTOs;
            //读取缓存
            List<FriendLinkDTO> friendLinkDTOsR = await RedisHelper.GetAsync<List<FriendLinkDTO>>("ZswBlog:Link:FriendLinkDTOList");
            if (friendLinkDTOsR != null)
            {
                friendLinkDTOs = friendLinkDTOsR;
            }
            else
            {
                //开启缓存
                List<FriendLink> friendLinks = await friendLinkSerivce.GetFriendLinksByIsShowAsync(true);
                friendLinkDTOs = await friendLinkMapper.MapperToDTOsAsync(friendLinks);
                await RedisHelper.SetAsync("ZswBlog:Link:FriendLinkDTOList", friendLinkDTOs, 600);
            }
            return Ok(friendLinkDTOs);
        }

        /// <summary>
        /// 申请友情链接
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddFriendLink(FriendLink param)
        {
            string str;
            int Code;
            param.Friendlink = System.Web.HttpUtility.HtmlEncode(param.Friendlink);
            param.FriendlinkImage = System.Web.HttpUtility.HtmlEncode(param.FriendlinkImage);
            param.FriendlinkIntroduce = System.Web.HttpUtility.HtmlEncode(param.FriendlinkIntroduce);
            param.FriendlinkTitle = System.Web.HttpUtility.HtmlEncode(param.FriendlinkTitle);
            param.FriendlinkCreateTime = DateTime.Now;
            param.IsShow = 0;
            Code = await friendLinkSerivce.AddEntityAsync(param) ? 200 : 500;
            str = Code == 200 ? "申请成功" : "申请失败";
            return Ok(new { code = Code, msg = str });
        }
    }
}
