using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.Common.Util;
using ZswBlog.Core.config;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;
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

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="articleService"></param>
        /// <param name="mapper"></param>
        /// <param name="articleTagService"></param>
        public ArticleController(IArticleService articleService, IMapper mapper, IArticleTagService articleTagService)
        {
            _articleService = articleService;
            _mapper = mapper;
            _articleTagService = articleTagService;
        }


        /// <summary>
        /// 后台管理-分页获取文章列表
        /// </summary>
        /// <param name="limit">页码</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="categoryId">分类编码</param>
        /// <param name="nickTitle">模糊查询</param>
        /// <returns></returns>
        [Route("/api/article/admin/get/page")]
        [Authorize]
        [HttpGet]
        [FunctionDescription("后台管理-分页获取文章列表")]
        public async Task<ActionResult<PageDTO<ArticleDTO>>> GetArticleAllListByPage([FromQuery] int limit,
            [FromQuery] int pageIndex, [FromQuery] int categoryId, string nickTitle)
        {
            var articles = await _articleService.GetArticlesByPageAndIsShowAsync(limit, pageIndex, false);
            return Ok(articles);
        }

        /// <summary>
        /// 后台管理-文章更新
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        [Route("/api/article/admin/update")]
        [Authorize]
        [HttpPost]
        [FunctionDescription("后台管理-文章更新")]
        public async Task<ActionResult<bool>> UpdateArticle(ArticleUpdateQuery article)
        {
            var articleDetail = await _articleService.GetArticleEntityByIdAsync(article.id);
            var articleEntity = _mapper.Map(article, articleDetail);
            var replaceContent = StringHelper.ReplaceTag(article.content, 99999);
            articleEntity.lastUpdateDate = DateTime.Now;
            articleEntity.textCount = replaceContent.Length;
            articleEntity.readTime = replaceContent.Length / 325;
            var flag = await _articleService.UpdateEntityAsync(articleEntity);
            //删除所有文章标签
            await _articleTagService.RemoveAlreadyExistArticleTagAsync(article.id);
            //遍历添加文章标签
            foreach (var id in article.tagIdList)
            {
                await _articleTagService.AddEntityAsync(new ArticleTagEntity()
                {
                    articleId = articleEntity.id,
                    createDate = DateTime.Now,
                    tagId = id,
                    operatorId = -1
                });
            }

            return Ok(flag);
        }

        /// <summary>
        /// 后台管理-保存文章
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        [Route(template: "/api/article/admin/save")]
        [Authorize]
        [HttpPost]
        [FunctionDescription("后台管理-保存文章")]
        public async Task<ActionResult<bool>> SaveArticle(ArticleSaveQuery article)
        {
            var articleEntity = _mapper.Map<ArticleEntity>(article);
            var replaceContent = StringHelper.ReplaceTag(article.content, 99999);
            //设置文章基本参数
            articleEntity.like = 0;
            articleEntity.visits = 0;
            articleEntity.createDate = DateTime.Now;
            articleEntity.textCount = replaceContent.Length;
            articleEntity.readTime = replaceContent.Length / 325;
            articleEntity.operatorId = -1;
            var flag = await _articleService.AddEntityAsync(articleEntity);
            //遍历添加文章标签
            foreach (var id in article.tagIdList)
            {
                _articleTagService.AddEntityAsync(new ArticleTagEntity()
                {
                    articleId = articleEntity.id,
                    createDate = articleEntity.createDate,
                    tagId = id,
                    operatorId = -1
                });
            }

            return Ok(flag);
        }

        /// <summary>
        /// 后台管理-获取文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route(template: "/api/article/admin/get/{id}")]
        [Authorize]
        [HttpGet]
        [FunctionDescription("后台管理-获取文章详情")]
        public async Task<ActionResult<ArticleDTO>> GetAdminArticleById(int id)
        {
            var article = await _articleService.GetArticleByIdAsync(id, false, false);
            return Ok(article);
        }

        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="id">文章Id</param>
        /// <returns></returns>
        [Route(template: "/api/article/get/{id}")]
        [HttpGet]
        [FunctionDescription("获取文章详情")]
        public async Task<ActionResult<ArticleDTO>> GetArticleById(int id)
        {
            // var article = await RedisHelper.GetAsync<ArticleDTO>("ZswBlog:Article:Article-" + id);
            // if (article != null) return Ok(article);
            var article =  await _articleService.GetArticleByIdAsync(id, true, true);
            if (article == null)
            {
                return NotFound("未找到该文章，请重新返回浏览");
            }
            // await RedisHelper.SetAsync("ZswBlog:Article:Article-" + article.id, article, 60 * 60 * 6);
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
        [FunctionDescription("分页获取文章列表")]
        public async Task<ActionResult<PageDTO<ArticleDTO>>> GetArticleListByPage([FromQuery] int limit,
            [FromQuery] int pageIndex)
        {
            var articles = await _articleService.GetArticlesByPageAndIsShowAsync(limit, pageIndex, true);
            return Ok(articles);
        }


        /// <summary>
        /// 文章添加喜爱数
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [Route(template: "/api/article/save/like/{articleId}")]
        [HttpPost]
        [FunctionDescription("文章添加喜爱数")]
        public async Task<ActionResult<bool>> AddArticleLike(int articleId)
        {
            return await Task.Run(() => _articleService.AddArticleLikeAsync(articleId));
        }

        /// <summary>
        /// 根据喜好获取文章
        /// </summary>
        /// <returns></returns>
        [Route(template: "/api/article/get/list/like")]
        [HttpGet]
        [FunctionDescription("根据喜好获取文章")]
        public async Task<ActionResult<List<ArticleDTO>>> GetArticleListByLikes()
        {
            var articles = await _articleService.GetArticlesByLikeAsync(7);
            return Ok(articles);
        }

        /// <summary>
        /// 根据浏览数获取文章
        /// </summary>
        /// <returns></returns>
        [Route(template: "/api/article/get/list/visit")]
        [HttpGet]
        [FunctionDescription("根据浏览数获取文章")]
        public async Task<ActionResult<List<ArticleDTO>>> GetArticleListByVisit()
        {
            var articles = await _articleService.GetArticlesByVisitAsync(7);
            return Ok(articles);
        }

        /// <summary>
        /// 模糊查询获取文章
        /// </summary>
        /// <returns></returns>
        [Route(template: "/api/article/get/fuzzy")]
        [HttpGet]
        [FunctionDescription("模糊查询获取文章")]
        public async Task<ActionResult<List<ArticleDTO>>> GetArticleListByFuzzyTitle(string fuzzyTitle)
        {
            var articles = await _articleService.GetArticlesByDimTitleAsync(fuzzyTitle);
            return Ok(articles);
        }

        /// <summary>
        /// 根据文章类型分页获取文章列表
        /// </summary>
        /// <returns></returns>
        [Route(template: "/api/article/get/page/category")]
        [HttpGet]
        [FunctionDescription("根据文章类型分页获取文章列表")]
        public async Task<ActionResult<PageDTO<ArticleDTO>>> GetArticleListByCategory([FromQuery] int limit,
            [FromQuery] int pageIndex, [FromQuery] int categoryId)
        {
            var articles = await _articleService.GetArticleListByCategoryIdAsync(limit, pageIndex, categoryId);
            return Ok(articles);
        }
    }
}