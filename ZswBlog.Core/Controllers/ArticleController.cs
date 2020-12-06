using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.Common.Util;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;
using ZswBlog.Query;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 文章控制器
    /// </summary>
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;
        private readonly IArticleTagService _articleTagService;

        public ArticleController(IArticleService articleService, IMapper mapper, IArticleTagService articleTagService)
        {
            _articleService = articleService;
            _mapper = mapper;
            _articleTagService = articleTagService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        [Route(template: "/api/article/save")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<bool>> SaveArticle(ArticleSaveQuery article) {
            return await Task.Run(() =>
            {
                ArticleEntity articleEntity = _mapper.Map<ArticleEntity>(article);
                string replaceContent =  StringHelper.ReplaceTag(article.content, 99999);
                //设置文章基本参数
                articleEntity.like = 0;
                articleEntity.visits = 0;
                articleEntity.createDate = DateTime.Now;
                articleEntity.textCount = replaceContent.Length;
                articleEntity.readTime = replaceContent.Length / 125;
                articleEntity.operatorId = -1;
                bool flag = _articleService.AddEntity(articleEntity);
                //遍历添加文章标签
                foreach (int id in article.tagIdList)
                {
                    _articleTagService.AddEntity(new ArticleTagEntity()
                    {
                        articleId = articleEntity.id,
                        createDate = articleEntity.createDate,
                        tagId = id,
                        operatorId = -1
                    });
                }
                return Ok(flag);
            });
        }

        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id">文章Id</param>
        /// <returns></returns>
        [Route(template: "/api/article/get/{id}")]
        [HttpGet]
        public async Task<ActionResult<ArticleDTO>> GetArticleById(int id)
        {
            ArticleDTO article;
            article = await RedisHelper.GetAsync<ArticleDTO>("ZswBlog:Article:Article-" + id);
            if (article == null)
            {
                article = _articleService.GetArticleById(id);
                if (article == null) {
                    return NotFound("未找到该文章，请重新返回浏览");
                }
                await RedisHelper.SetAsync("ZswBlog:Article:Article-" + article.id, article, 60 * 60 * 6);
            }
            return Ok(article);
        }

        /// <summary>
        /// 分页获取文章列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [Route(template: "/api/article/get/page")]
        [HttpGet]
        public async Task<ActionResult<PageDTO<ArticleDTO>>> GetArticleListByPage([FromQuery] int limit, [FromQuery] int pageIndex)
        {
            return await Task.Run(() =>
            {
                PageDTO<ArticleDTO> articles = _articleService.GetArticlesByPageAndIsShow(limit, pageIndex, true);
                return Ok(articles);
            });
        }
        /// <summary>
        /// 文章添加喜爱数
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [Route(template: "/api/article/save/like/{articleId}")]
        [HttpPost]
        public async Task<ActionResult<bool>> AddArticleLike(int articleId) {
            return await Task.Run(() =>
            {
                return _articleService.AddArticleLike(articleId);
            });
        }

        /// <summary>
        /// 根据喜好获取文章
        /// </summary>
        /// <returns></returns>
        [Route(template: "/api/article/get/list/like")]
        [HttpGet]
        public async Task<ActionResult<List<ArticleDTO>>> GetArticleListByLikes()
        {
            return await Task.Run(() =>
            {
                List<ArticleDTO> articles = _articleService.GetArticlesByLike(7);
                return Ok(articles);
            });
        }

        /// <summary>
        /// 根据浏览数获取文章
        /// </summary>
        /// <returns></returns>
        [Route(template: "/api/article/get/list/visit")]
        [HttpGet]
        public async Task<ActionResult<List<ArticleDTO>>> GetArticleListByVisit()
        {
            return await Task.Run(() =>
            {
                List<ArticleDTO> articles = _articleService.GetArticlesByVisit(7);
                return Ok(articles);
            });
        }

        /// <summary>
        /// 模糊查询获取文章
        /// </summary>
        /// <returns></returns>
        [Route(template: "/api/article/get/fuzzy")]
        [HttpGet]
        public async Task<ActionResult<List<ArticleDTO>>> GetArticleListByFuzzyTitle(string fuzzyTitle)
        {
            return await Task.Run(() =>
            {
                List<ArticleDTO> articles = _articleService.GetArticlesByDimTitle(fuzzyTitle);
                return Ok(articles);
            });
        }

        /// <summary>
        /// 根据文章类型分页获取文章列表
        /// </summary>
        /// <returns></returns>
        [Route(template: "/api/article/get/page/category")]
        [HttpGet]
        public async Task<ActionResult<PageDTO<ArticleDTO>>> GetArticleListByCategory([FromQuery] int limit, [FromQuery] int pageIndex, [FromQuery] int categoryId)
        {
            return await Task.Run(() =>
            {
                PageDTO<ArticleDTO> articles = _articleService.GetArticleListByCategoryId(limit, pageIndex, categoryId);
                return Ok(articles);
            });
        }
    }
}

