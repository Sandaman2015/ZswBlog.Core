﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.Common;
using ZswBlog.DTO;
using ZswBlog.Entity;
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
        [Authorize]
        [FunctionDescription("获取所有的通知公告")]
        public async Task<ActionResult<List<AnnouncementDTO>>> GetAllAnnouncements()
        {
            var announcements = await _announcementService.GetAllAnnouncementAsync();
            return Ok(announcements);
        }

        /// <summary>
        /// 后台管理-分页获取通知公告列表
        /// </summary>
        /// <param name="limit">页码</param>
        /// <param name="pageIndex">页数</param>
        /// <returns></returns>
        [Route("/api/announcement/admin/get/page")]
        [Authorize]
        [HttpGet]
        [FunctionDescription("后台管理-分页获取通知公告列表")]
        public async Task<ActionResult<PageDTO<AnnouncementDTO>>> GetAnnouncementListByPage([FromQuery] int limit,
            [FromQuery] int pageIndex)
        {
            var announcementPage = await _announcementService.GetAnnouncementAsyncByPage(pageIndex, limit);
            return Ok(announcementPage);
        }

        /// <summary>
        /// 添加通知公告对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Route("/api/announcement/admin/save")]
        [Authorize]
        [HttpPost]
        [FunctionDescription("后台管理-添加通知公告")]
        public ActionResult<bool> SaveAnnouncement([FromBody] AnnouncementEntity entity)
        {
            entity.createDate = DateTime.Now;
            entity.isShow = true;
            entity.operatorId = -1;
            var flag = _announcementService.AddEntity(entity);
            return Ok(flag);
        }

        /// <summary>
        /// 根据主键删除通知公告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/api/announcement/admin/remove/{id}")]
        [Authorize]
        [HttpDelete]
        [FunctionDescription("后台管理-删除通知公告")]
        public ActionResult<bool> RemoveAnnouncement([FromRoute] int id)
        {
            AnnouncementEntity entity = new AnnouncementEntity
            {
                id = id
            };
            var flag = _announcementService.RemoveAnnouncementById(entity);
            return Ok(flag);
        }
    }
}