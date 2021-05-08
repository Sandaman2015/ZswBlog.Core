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
    /// 本站标签
    /// </summary>
    [ApiController]
    public class SiteTagController : ControllerBase
    {
        private readonly ISiteTagService _siteTagService;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="siteTagService"></param>
        public SiteTagController(ISiteTagService siteTagService)
        {
            _siteTagService = siteTagService;
        }

        /// <summary>
        /// 获取所有站点标签
        /// </summary>
        /// <returns></returns>
        [Route("/api/sitetag/get/all")]
        [HttpGet]
        [FunctionDescription("获取所有站点标签")]
        public async Task<ActionResult<List<SiteTagDTO>>> GetAllSiteTagAsync()
        {
            var siteTagDtoList = await _siteTagService.GetAllSiteTagsAsync();
            //siteTagDTOList = await RedisHelper.GetAsync<List<SiteTagDTO>>("ZswBlog:SiteTag:SiteTagList");
            //if (siteTagDTOList == null)
            //{
            //    await RedisHelper.SetAsync("ZswBlog:SiteTag:SiteTagList", siteTagDTOList, 1200);
            //}
            return Ok(siteTagDtoList);
        }

        /// <summary>
        /// 添加站点标签
        /// </summary>
        /// <param name="param">保存参数</param>
        /// <returns></returns>
        [Route("/api/siteTag/save")]
        [HttpPost]
        [FunctionDescription("添加站点标签")]
        public async Task<ActionResult<bool>> SaveSiteTag(SiteTagEntity param)
        {
            param.createDate = DateTime.Now;
            param.isShow = true;
            param.like = 0;
            param.title = System.Web.HttpUtility.HtmlEncode(param.title);
            var flag = await _siteTagService.AddEntityAsync(param);
            return Ok(flag);
        }

        /// <summary>
        /// 删除站点标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/api/siteTag/remove/{id}")]
        [HttpDelete]
        [Authorize]
        [FunctionDescription("删除站点标签")]
        public async Task<ActionResult<bool>> RemoveSiteTag([FromRoute]int id) {
            SiteTagEntity tagEntity = new SiteTagEntity() {
                id = id
            };
           return Ok(await _siteTagService.RemoveSiteTagById(tagEntity));
        }
    }
}