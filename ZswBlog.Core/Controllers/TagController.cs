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
    /// 标签页
    /// </summary>
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IArticleTagService _articleTagService;
        private readonly ITagService _tagService;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="articleTagService"></param>
        /// <param name="tagService"></param>
        public TagController(IArticleTagService articleTagService, ITagService tagService)
        {
            _articleTagService = articleTagService;
            _tagService = tagService;
        }

        /// <summary>
        /// 获取所有的文章标签
        /// </summary>
        /// <returns></returns>
        [Route("/api/tag/get/all")]
        [HttpGet]
        [FunctionDescription("获取所有的文章标签")]
        public async Task<ActionResult<List<TagDTO>>> GetTagList()
        {
            var articleTags = await _tagService.GetAllTagAsync();
            return Ok(articleTags);
            //articleTags = await RedisHelper.GetAsync<List<TagDTO>>("ZswBlog:Tag:TagList");
            //if (articleTags == null)
            //{
            //获取所有标签
            //开启缓存
            //    await RedisHelper.SetAsync("ZswBlog:Tag:TagList", articleTags, 1200);
            //}
        }

        /// <summary>
        /// 分页获取根据Id单个标签下的文章
        /// </summary>
        /// <param name="limit">页码</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="tagId">类型编码</param>
        /// <returns></returns>
        [Route(template: "/api/tag/get/page/{tagId}")]
        [HttpGet]
        [FunctionDescription("分页获取根据Id单个标签下的文章")]
        public async Task<ActionResult<PageDTO<ArticleDTO>>> GetArticleListByPageAndTagId(int limit, int pageIndex,
            [FromRoute] int tagId)
        {
            var pageDto = await _articleTagService.GetArticleListIdByTagIdAsync(limit, pageIndex, tagId);
            return Ok(pageDto);
        }

        /// <summary>
        /// 保存文章标签
        /// </summary>
        /// <param name="tagEntity">标签对象</param>
        /// <returns></returns>
        [Route(template: "/api/tag/admin/save")]
        [HttpPost]
        [Authorize]
        [FunctionDescription("新增文章标签")]
        public ActionResult<bool> SaveTag([FromBody] TagEntity tagEntity)
        {
            tagEntity.createDate = DateTime.Now;
            tagEntity.operatorId = -1;
            return Ok(_tagService.AddEntity(tagEntity));
        }

        /// <summary>
        /// 删除文章标签
        /// </summary>
        /// <param name="tagId">标签对象Id</param>
        /// <returns></returns>
        [Route(template: "/api/tag/admin/remove/{tagId}")]
        [HttpDelete]
        [Authorize]
        [FunctionDescription("删除文章标签")]
        public ActionResult<bool> RemoveTag([FromRoute] int tagId)
        {
            return Ok(_tagService.RemoveTagById(tagId));
        }
    }
}