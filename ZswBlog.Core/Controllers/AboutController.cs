//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using ZswBlog.DTO;
//using ZswBlog.Entity;
//using ZswBlog.IServices;
//using ZswBlog.MapperFactory;

//namespace ZswBlog.Web.Controllers
//{
//    /// <summary>
//    /// 关于页
//    /// </summary>
//    [Route("/api/[controller]/[action]")]
//    [ApiController]
//    public class AboutController : ControllerBase
//    {
//        private readonly ISiteTagService siteTagService;
//        private readonly ITimeLineService timeLineService;
//        private readonly SiteTagMapper siteTagMapper;
//        private readonly TimeLineMapper timeLineMapper;
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="siteTagService"></param>
//        /// <param name="timeLineService"></param>
//        /// <param name="siteTagMapper"></param>
//        /// <param name="timeLineMapper"></param>
//        public AboutController(ISiteTagService siteTagService, ITimeLineService timeLineService, SiteTagMapper siteTagMapper, TimeLineMapper timeLineMapper)
//        {
//            this.siteTagService = siteTagService;
//            this.timeLineService = timeLineService;
//            this.siteTagMapper = siteTagMapper;
//            this.timeLineMapper = timeLineMapper;
//        }
//        /// <summary>
//        /// 获取所有站点标签
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet]
//        public async Task<ActionResult<List<SiteTagDTO>>> GetAllSiteTagAsync()
//        {
//            List<SiteTagDTO> SiteTagDTOs;
//            List<SiteTagDTO> SiteTagDTOsR = await RedisHelper.GetAsync<List<SiteTagDTO>>("ZswBlog:About:SiteTagDTOList");
//            if (SiteTagDTOsR != null)
//            {
//                SiteTagDTOs = SiteTagDTOsR;
//            }
//            else
//            {
//                List<SiteTag> siteTags = await siteTagService.GetSiteTagsByIsShowAsync(true);
//                SiteTagDTOs = await siteTagMapper.MapperToDTOsAsync(siteTags);
//                await RedisHelper.SetAsync("ZswBlog:About:SiteTagDTOList", SiteTagDTOs, 1200);
//            }
//            return Ok(SiteTagDTOs);
//        }
//        /// <summary>
//        /// 获取所有时间线文章
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet]
//        public async Task<ActionResult<List<TimeLineDTO>>> GetAllTimeLineAsync()
//        {
//            List<TimeLineDTO> timelinesDTOs;
//            List<TimeLineDTO> TimeLineDTOR = await RedisHelper.GetAsync<List<TimeLineDTO>>("ZswBlog:About:TimeLineDTOList");
//            if (TimeLineDTOR != null)
//            {
//                timelinesDTOs = TimeLineDTOR;
//            }
//            else
//            {
//                List<Timeline> timelines = await timeLineService.GetAllTimeLinesOrderByTimeAsync();
//                timelinesDTOs = await timeLineMapper.MapperToDTOsAsync(timelines);
//                await RedisHelper.SetAsync("ZswBlog:About:TimeLineDTOList", timelinesDTOs, 1200);
//            }
//            return Ok(timelinesDTOs);
//        }
//        /// <summary>
//        /// 添加站点标签
//        /// </summary>
//        /// <param name="prama"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public async Task<ActionResult> AddSiteTagAsync(SiteTag prama)
//        {
//            int Code;
//            prama.SitetagCreateTime = DateTime.Now;
//            prama.IsShow = false;
//            prama.SitetagLikes = 0;
//            prama.SitetagTitle = System.Web.HttpUtility.HtmlEncode(prama.SitetagTitle);
//            Code = await siteTagService.AddEntityAsync(prama) ? 200 : 500;
//            return Ok(new { code = Code });
//        }
//    }
//}
