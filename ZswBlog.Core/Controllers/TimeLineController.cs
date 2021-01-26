using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.Core.config;
using ZswBlog.DTO;
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
        public ActionResult<List<TimeLineDTO>> GetAllTimeLineAsync()
        {
            var timelinesDtoList = _timeLineService.GetTimeLineListAsync();
            //timelinesDTOList = await RedisHelper.GetAsync<List<TimeLineDTO>>("ZswBlog:TimeLine:TimeLineList");
            //if (timelinesDTOList==null)
            //{
            //    timelinesDTOList = _timeLineService.GetTimeLineList();
            //    await RedisHelper.SetAsync("ZswBlog:TimeLine:TimeLineList", timelinesDTOList, 1200);
            //}
            return Ok(timelinesDtoList);
        }
    }
}
