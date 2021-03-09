using System;
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
        [Route("/api/travel/get/page")]
        [HttpGet]
        [FunctionDescription("分页获取旅行分享信息")]
        public async Task<ActionResult<PageDTO<TravelDTO>>> GetTravelsByPage(int limit, int pageIndex)
        {
            var travelPageDto = await _travelService.GetTravelsByPageAsync(limit, pageIndex, true);
            return Ok(travelPageDto);
        }

        /// <summary>
        /// 分页获取旅行信息
        /// </summary>
        /// <param name="limit">页码</param>
        /// <param name="pageIndex">页数</param>
        /// <returns></returns>
        [Route("/api/travel/admin/get/page")]
        [HttpGet]
        [FunctionDescription("后台管理-分页获取旅行分享信息")]
        public async Task<ActionResult<PageDTO<TravelDTO>>> GetAdminTravelsByPage(int limit, int pageIndex)
        {
            var travelPageDto = await _travelService.GetTravelsByPageAsync(limit, pageIndex, false);
            return Ok(travelPageDto);
        }

        /// <summary>
        /// 后台管理保存旅行分享
        /// </summary>
        /// <param name="entity">保存对象</param>
        /// <returns></returns>
        [Route("/api/travel/admin/save")]
        [HttpGet]
        [FunctionDescription("后台管理-保存分享信息")]
        public async Task<ActionResult<bool>> SaveTralvel(TravelEntity entity)
        {
            entity.createDate = DateTime.Now;
            entity.isShow = true;
            entity.operatorId = -1;
            bool flag = await _travelService.AddEntityAsync(entity);
            return Ok(flag);
        }

        /// <summary>
        /// 后台管理删除分享信息
        /// </summary>
        /// <param name="id">分享编码</param>
        /// <returns></returns>
        [Route("/api/travel/admin/remove/{id}")]
        [HttpGet]
        [FunctionDescription("后台管理-删除分享信息")]
        public async Task<ActionResult<bool>> RremoveTralvel([FromRoute]int id)
        {
            bool flag = await _travelService.RemoveTravelAsync(id);
            return Ok(flag);
        }
    }
}