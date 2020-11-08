using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;

namespace ZswBlog.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteTagController : ControllerBase
    {
        private readonly ISiteTagService _siteTagService;
        public SiteTagController(ISiteTagService siteTagService)
        {
            _siteTagService = siteTagService;
        }
        /// <summary>
        /// 获取所有站点标签
        /// </summary>
        /// <returns></returns>
        [Route("/sitetag/get/all")]
        [HttpGet]
        public async Task<ActionResult<List<SiteTagDTO>>> GetAllSiteTagAsync()
        {
            List<SiteTagDTO> siteTagDTOList = _siteTagService.GetAllSiteTags();
            //siteTagDTOList = await RedisHelper.GetAsync<List<SiteTagDTO>>("ZswBlog:SiteTag:SiteTagList");
            //if (siteTagDTOList == null)
            //{
            //    await RedisHelper.SetAsync("ZswBlog:SiteTag:SiteTagList", siteTagDTOList, 1200);
            //}
            return Ok(siteTagDTOList);

        }

        /// <summary>
        /// 添加站点标签
        /// </summary>
        /// <param name="prama"></param>
        /// <returns></returns>
        [Route("/sitetag/save")]
        [HttpPost]
        public async Task<ActionResult<bool>> SaveSiteTag(SiteTagEntity prama)
        {
            return await Task.Run(() =>
            {
                prama.createDate = DateTime.Now;
                prama.isShow = false;
                prama.like = 0;
                prama.title = System.Web.HttpUtility.HtmlEncode(prama.title);
                bool flag = _siteTagService.AddEntity(prama);
                return Ok(flag);
            });
        }
    }
}
