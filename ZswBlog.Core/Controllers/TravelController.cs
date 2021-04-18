using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.Common;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;
using ZswBlog.Query;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 旅行分享
    /// </summary>
    [ApiController]
    public class TravelController : ControllerBase
    {
        private readonly ITravelService _travelService;
        private readonly IMapper _mapper;
        private readonly ITravelFileAttachmentService _travelFileAttachmentService;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="travelService"></param>
        /// <param name="mapper"></param>
        /// <param name="travelFileAttachmentService"></param>
        public TravelController(ITravelService travelService, IMapper mapper, ITravelFileAttachmentService travelFileAttachmentService)
        {
            _travelService = travelService;
            _mapper = mapper;
            _travelFileAttachmentService = travelFileAttachmentService;
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
        /// 根据id获取分享详情
        /// </summary>
        /// <param name="id">分享详情</param>
        /// <returns></returns>
        [Route("/api/travel/admin/get/{id}")]
        [HttpGet]
        [FunctionDescription("后台管理-根据主键编码获取分享详情")]
        public async Task<ActionResult<TravelDTO>> GetAdminTravel([FromRoute] int id) {
           TravelDTO travelDTO =  await _travelService.GetTravelByIdAsync(id);
            return Ok(travelDTO);
        }

        /// <summary>
        /// 后台管理保存旅行分享
        /// </summary>
        /// <param name="query">保存对象</param>
        /// <returns></returns>
        [Route("/api/travel/admin/save")]
        [HttpPost]
        [FunctionDescription("后台管理-保存分享信息")]
        public async Task<ActionResult<bool>> SaveTralvel(TravelSaveQuery query)
        {
            TravelEntity entity = _mapper.Map<TravelEntity>(query);
            entity.createDate = DateTime.Now;
            entity.isShow = true;
            entity.operatorId = -1;
            bool flag = await _travelService.AddEntityAsync(entity);
            //保存附件关联
            if (query.fileList.Length > 0)
            {
                foreach (var item in query.fileList)
                {
                    await _travelFileAttachmentService.AddEntityAsync(new TravelFileAttachmentEntity
                    {
                        createDate = DateTime.Now,
                        fileAttachmentId = item,
                        travelId = entity.id,
                        operatorId = -1
                    });
                }
            }
            return Ok(flag);
        }


        /// <summary>
        /// 后台管理更新旅行分享
        /// </summary>
        /// <param name="query">更新对象</param>
        /// <returns></returns>
        [Route("/api/travel/admin/update")]
        [HttpPost]
        [FunctionDescription("后台管理-更新分享信息")]
        public async Task<ActionResult<bool>> UpdateTralvel(TravelSaveQuery query)
        {
            TravelEntity entity = _mapper.Map<TravelEntity>(query);
            entity.operatorId = -1;
            entity.createDate = DateTime.Now;
            bool flag = await _travelService.UpdateEntityAsync(entity);
            //保存附件关联
            if (query.fileList.Length > 0)
            {
                await _travelFileAttachmentService.RemoveAllTravelRelationAsync(entity.id);
                foreach (var item in query.fileList) {
                    await _travelFileAttachmentService.AddEntityAsync(new TravelFileAttachmentEntity
                    {
                        createDate = DateTime.Now,
                        fileAttachmentId = item,
                        travelId = query.id,
                        operatorId = -1
                    });
                }
            }
            return Ok(flag);
        }

        /// <summary>
        /// 后台管理删除分享信息
        /// </summary>
        /// <param name="id">分享编码</param>
        /// <returns></returns>
        [Route("/api/travel/admin/remove/{id}")]
        [HttpDelete]
        [FunctionDescription("后台管理-删除分享信息")]
        public async Task<ActionResult<bool>> RremoveTralvel([FromRoute]int id)
        {
            bool flag = await _travelService.RemoveTravelAsync(id);
            return Ok(flag);
        }
    }
}