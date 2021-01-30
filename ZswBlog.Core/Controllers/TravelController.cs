using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.Common;
using ZswBlog.Core.config;
using ZswBlog.DTO;
using ZswBlog.IServices;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 旅行分享
    /// </summary>
    [ApiController]
    public class TravelController : ControllerBase
    {
        private readonly ITravelService _travelService;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="travelService"></param>
        public TravelController(ITravelService travelService)
        {
            _travelService = travelService;
        }

        /// <summary>
        /// 分页获取旅行信息
        /// </summary>
        /// <param name="limit">页码</param>
        /// <param name="pageIndex">页数</param>
        /// <returns></returns>
        [Route("/api/travel/page/get")]
        [HttpGet]
        [FunctionDescription("分页获取旅行分享信息")]
        public async Task<ActionResult<PageDTO<TravelDTO>>> GetTravelsByPage(int limit, int pageIndex)
        {
            var travelPageDto = await _travelService.GetTravelsByPageAsync(limit, pageIndex);
            return Ok(travelPageDto);
        }
    }
}