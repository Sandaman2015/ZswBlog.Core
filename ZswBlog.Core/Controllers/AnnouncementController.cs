using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ZswBlog.Core.config;
using ZswBlog.DTO;
using ZswBlog.IServices;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 通知公告
    /// </summary>
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="announcementService"></param>
        [Description]
        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        /// <summary>
        /// 获取指定置顶的通知公告
        /// </summary>
        /// <returns></returns>
        [Route("/api/announcement/get/top")]
        [HttpGet]
        [FunctionDescription("获取指定置顶的通知公告")]
        public async Task<ActionResult<List<AnnouncementDTO>>> GetAnnouncementsOnTop()
        {
            const int count = 3;
            var announcements = await _announcementService.GetAnnouncementsOnTopAsync(count);
            return Ok(announcements);
        }

        /// <summary>
        /// 获取正在推送的通知公告
        /// </summary>
        /// <returns></returns>
        [Route(("/api/announcement/get/push"))]
        [HttpGet]
        [FunctionDescription("获取正在推送的通知公告")]
        public async Task<ActionResult<List<AnnouncementDTO>>> GetPushAnnouncements()
        {
            var announcements = await _announcementService.GetPushAnnouncementAsync();
            return Ok(announcements);
        }

        /// <summary>
        /// 获取所有的通知公告
        /// </summary>
        /// <returns></returns>
        [Route(("/api/announcement/get/all"))]
        [HttpGet]
        [FunctionDescription("获取所有的通知公告")]
        public async Task<ActionResult<List<AnnouncementDTO>>> GetAllAnnouncements()
        {
            var announcements = await _announcementService.GetAllAnnouncementAsync();
            return Ok(announcements);
        }
    }
}