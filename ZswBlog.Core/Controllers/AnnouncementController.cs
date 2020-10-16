using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.DTO;
using ZswBlog.IServices;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 通知公告
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        /// <summary>
        /// 获取指定置顶的通知公告
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        [Route("/announcement/get/top")]
        [HttpGet]
        public async Task<ActionResult<List<AnnouncementDTO>>> GetAnnouncementsOnTop()
        {
            return await Task.Run(() =>
            {
                int count = 3;
                List<AnnouncementDTO> announcements = _announcementService.GetAnnouncementsOnTop(count);
                return Ok(announcements);
            });
        }

        /// <summary>
        /// 获取正在推送的通知公告
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        [Route("/announcement/get/push")]
        [HttpGet]
        public async Task<ActionResult<List<AnnouncementDTO>>> GetPushAnnouncements()
        {
            return await Task.Run(() =>
            {
                List<AnnouncementDTO> announcements = _announcementService.GetPushAnnouncement();
                return Ok(announcements);
            });
        }

        /// <summary>
        /// 获取所有的通知公告
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        [Route("/announcement/get/all")]
        [HttpGet]
        public async Task<ActionResult<List<AnnouncementDTO>>> GetAllAnnouncements()
        {
            return await Task.Run(() =>
            {
                List<AnnouncementDTO> announcements = _announcementService.GetAllAnnouncement();
                return Ok(announcements);
            });
        }
    }
}
