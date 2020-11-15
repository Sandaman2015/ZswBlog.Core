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
    /// 旅行分享
    /// </summary>
    [ApiController]
    public class TravelController : ControllerBase
    {
        private ITravelService _travelService;
        public TravelController(ITravelService travelService)
        {
            _travelService = travelService;
        }

        /// <summary>
        /// 分页获取旅行信息
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [Route("/api/travel/page/get")]
        [HttpGet]
        public async Task<ActionResult<PageDTO<TravelDTO>>> GetTravelsByPage(int limit,int pageIndex)
        {
            return await Task.Run(() =>
            {
                PageDTO<TravelDTO> travelPageDTO = _travelService.GetTravelsByPage(limit, pageIndex);
                return Ok(travelPageDTO);
            });
        }
    }
}
