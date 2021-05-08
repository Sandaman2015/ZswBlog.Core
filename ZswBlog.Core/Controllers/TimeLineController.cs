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
    /// 时间线
    /// </summary>
    [ApiController]
    public class TimeLineController : ControllerBase
    {
        private readonly ITimeLineService _timeLineService;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="timeLineService"></param>
        public TimeLineController(ITimeLineService timeLineService)
        {
            _timeLineService = timeLineService;
        }

        /// <summary>
        /// 获取所有时间线文章
        /// </summary>
        /// <returns></returns>
        [Route("/api/timeline/get/all")]
        [HttpGet]
        [FunctionDescription("获取所有时间线文章")]
        public async Task<ActionResult<List<TimeLineDTO>>> GetAllTimeLineAsync()
        {
            var timelinesDtoList = await _timeLineService.GetTimeLineListAsync();
            return Ok(timelinesDtoList);
            //timelinesDTOList = await RedisHelper.GetAsync<List<TimeLineDTO>>("ZswBlog:TimeLine:TimeLineList");
            //if (timelinesDTOList==null)
            //{
            //    timelinesDTOList = _timeLineService.GetTimeLineList();
            //    await RedisHelper.SetAsync("ZswBlog:TimeLine:TimeLineList", timelinesDTOList, 1200);
            //}
        }

        /// <summary>
        /// 后台添加时间线
        /// </summary>
        /// <param name="timeLine">时间线</param>
        /// <returns></returns>
        [Route("/api/timeline/admin/save")]
        [HttpPost]
        [Authorize]
        [FunctionDescription("后台添加时间线")]
        public async Task<ActionResult<bool>> SaveTimeLine([FromBody]TimeLineEntity timeLine)
        {
            timeLine.createDate = DateTime.Now;
            var flag = await _timeLineService.AddEntityAsync(timeLine);
            return Ok(flag);
        }

        /// <summary>
        /// 后台删除时间线
        /// </summary>
        /// <param name="id">时间线id</param>
        /// <returns></returns>
        [Route("/api/timeline/admin/remove/{id}")]
        [HttpDelete]
        [Authorize]
        [FunctionDescription("后台删除时间线")]
        public async Task<ActionResult<bool>> RemoveTimeLine([FromRoute] int id)
        {
            var flag = await _timeLineService.RemoveEntityAsync(id);
            return Ok(flag);
        }
    }
}
