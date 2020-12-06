using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.IServices;

namespace ZswBlog.Web.Controllers
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
        /// 
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
        public async Task<ActionResult<List<TagDTO>>> GetTagList()
        {
            List<TagDTO> articleTags;
            return await Task.Run(() =>
            {
                articleTags = _tagService.GetAllTag();
                return Ok(articleTags);
            });
            //articleTags = await RedisHelper.GetAsync<List<TagDTO>>("ZswBlog:Tag:TagList");
            //if (articleTags == null)
            //{
                //获取所有标签
                //开启缓存
            //    await RedisHelper.SetAsync("ZswBlog:Tag:TagList", articleTags, 1200);
            //}
        }

        /// <summary>
        /// 获取根据Id单个标签下的文章
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [Route(template: "/api/tag/get/page/{tagId}")]
        [HttpGet]
        public async Task<ActionResult<PageDTO<ArticleDTO>>> GetArticleListByPageAndTagId(int limit, int pageIndex, [FromRoute] int tagId)
        {
            return await Task.Run(() =>
            {
                PageDTO<ArticleDTO> pageDTO = _articleTagService.GetArticleListIdByTagId(limit, pageIndex, tagId);
                return Ok(pageDTO);
            });
        }
    }
}
