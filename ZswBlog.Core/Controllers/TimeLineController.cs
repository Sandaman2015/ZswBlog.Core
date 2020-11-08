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
    [Route("api/[controller]")]
    [ApiController]
    public class TimeLineController : ControllerBase
    {
        private readonly ITimeLineService _timeLineService;

        public TimeLineController(ITimeLineService timeLineService)
        {
            _timeLineService = timeLineService;
        }

        /// <summary>
        /// 获取所有时间线文章
        /// </summary>
        /// <returns></returns>
        [Route("/timeline/get/all")]
        [HttpGet]
        public async Task<ActionResult<List<TimeLineDTO>>> GetAllTimeLineAsync()
        {
            List<TimeLineDTO> timelinesDTOList = _timeLineService.GetTimeLineList();
            //timelinesDTOList = await RedisHelper.GetAsync<List<TimeLineDTO>>("ZswBlog:TimeLine:TimeLineList");
            //if (timelinesDTOList==null)
            //{
            //    timelinesDTOList = _timeLineService.GetTimeLineList();
            //    await RedisHelper.SetAsync("ZswBlog:TimeLine:TimeLineList", timelinesDTOList, 1200);
            //}
            return Ok(timelinesDTOList);
        }
    }
}
