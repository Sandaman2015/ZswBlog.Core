using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZswBlog.Common;
using ZswBlog.Common.Exception;
using ZswBlog.Common.Util;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class ArticleService : BaseService<ArticleEntity, IArticleRepository>, IArticleService
    {
        public IArticleRepository ArticleRepository { get; set; }
        public IArticleTagService ArticleTagService { get; set; }
        public ICategoryService CategoryService { get; set; }
        public IMapper Mapper { get; set; }

        /// <summary>
        /// 分页根据显示获取文章
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="categoryId"></param>
        /// <param name="isShow"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<PageDTO<ArticleDTO>> GetArticleListByCategoryIdAsync(int limit, int pageIndex, int categoryId,
            bool isShow, string title)
        {
            Expression<Func<ArticleEntity, bool>> expression = t => true;
            if (isShow) expression = expression.And(ac => ac.isShow);
            if (!string.IsNullOrEmpty(title))
            {
                expression = expression.And(ac => ac.title.Contains(title));
            }
            if (categoryId > 0)
            {
                expression = expression.And(ac => ac.categoryId == categoryId);
            }
            var articles = ArticleRepository.GetModelsByPage(limit, pageIndex, false,
                a => a.lastUpdateDate, expression,
                out var pageCount);
            var articleDtoList = Mapper.Map<List<ArticleDTO>>(articles
                .Include(t => t.category).ToList());
            foreach (var articleDto in articleDtoList)
            {
                articleDto.tags = await ArticleTagService.GetTagListByArticleIdAsync(articleDto.id);
                articleDto.content = StringHelper.ReplaceTag(articleDto.content, 500);
            }

            return new PageDTO<ArticleDTO>(limit,
                pageIndex,
                pageCount,
                articleDtoList);
        }

        /// <summary>
        /// 根据文章标题模糊查询
        /// </summary>
        /// <param name="dimTitle"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        public async Task<List<ArticleDTO>> GetArticlesByDimTitleAsync(string dimTitle, bool isShow)
        {
            var articles = ArticleRepository.GetModels(a =>
                    a.title.Contains(dimTitle) && isShow ? a.isShow == true : a.id > 0).Include(c => c.category);
            var articleDtoList = Mapper.Map<List<ArticleDTO>>(articles.ToList().Where(a => a.isShow));
            foreach (var articleDto in articleDtoList)
            {
                articleDto.tags = await ArticleTagService.GetTagListByArticleIdAsync(articleDto.id);
                articleDto.content = StringHelper.ReplaceTag(articleDto.content, 500);
            }

            return articleDtoList;
        }

        /// <summary>
        /// 根据文章id获取文章DTO
        /// </summary>
        /// <param name="articleId">文章编码</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="addVisit">是否添加浏览数</param>
        /// <returns></returns>
        public async Task<ArticleDTO> GetArticleByIdAsync(int articleId, bool isShow, bool addVisit)
        {
            ArticleEntity article;
            if (isShow)
            {
                article = await ArticleRepository.GetSingleModelAsync(a => a.id == articleId && a.isShow);
            }
            else
            {
                article = await ArticleRepository.GetSingleModelAsync(a => a.id == articleId);
            }

            if (article == null)
            {
                throw new BusinessException("未找到文章", 404);
            }

            if (addVisit) await AddArticleVisitAsync(article);
            var articleDto = Mapper.Map<ArticleDTO>(article);
            articleDto.category = await CategoryService.GetCategoryByIdAsync(articleDto.categoryId);
            articleDto.tags = await ArticleTagService.GetTagListByArticleIdAsync(articleId);
            return articleDto;
        }

        public async Task<ArticleEntity> GetArticleEntityByIdAsync(int articleId)
        {
            var articleEntity = await ArticleRepository.GetSingleModelAsync(a => a.id == articleId);
            return articleEntity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="article"></param>
        private Task<bool> AddArticleVisitAsync(ArticleEntity article)
        {
            article.visits += 1;
            return Task.FromResult(ArticleRepository.Update(article));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<bool> AddArticleLikeAsync(int articleId)
        {
            var article = await ArticleRepository.GetSingleModelAsync(a => a.id == articleId);
            article.like += 1;
            return ArticleRepository.Update(article);
        }

        /// <summary>
        /// 分页获取文章DTO
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        public PageDTO<ArticleDTO> GetArticlesByPageAndIsShow(int limit, int pageIndex, int categoryId, bool isShow)
        {
            PageEntity<ArticleEntity> articles;
            Expression<Func<ArticleEntity, bool>> expression = a => true;
            if (categoryId != 0)
            {
                expression = expression.And(a => a.categoryId == categoryId);
            }
            if (isShow)
            {
                expression = expression.And(a => a.isShow == isShow);
            }
            articles = ArticleRepository.GetModelsByPage(limit, pageIndex, false, a => a.createDate, expression);
            articles.data
                .OrderBy(a => a.isTop)
                .ThenBy(a => a.topSort);
            var articleDtoList = Mapper.Map<List<ArticleDTO>>(articles.data.ToList());
            foreach (var articleDto in articleDtoList)
            {
                articleDto.content = StringHelper.ReplaceTag(articleDto.content, 500);
            }
            return new PageDTO<ArticleDTO>(limit,
                pageIndex,
                articles.count,
                articleDtoList);
        }

        /// <summary>
        /// 获取最近发布的文章
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<List<ArticleDTO>> GetArticlesByNearSaveAsync(int count)
        {
            return await Task.Run(() =>
            {
                var articles = ArticleRepository.GetModels(a => a.isShow).Include(c => c.category);
                var articleDtoList =
                    Mapper.Map<List<ArticleDTO>>(articles.OrderByDescending(a => a.createDate).Take(count).ToList());
                foreach (var articleDto in articleDtoList)
                {
                    articleDto.content = StringHelper.ReplaceTag(articleDto.content, 500);
                }
                return articleDtoList;
            });
        }

        /// <summary>
        /// 获取最喜爱的文章列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ArticleDTO>> GetArticlesByLikeAsync(int likeCount)
        {
            return await Task.Run(() =>
            {
                var articles = ArticleRepository.GetModels(a => a.isShow);
                return Mapper.Map<List<ArticleDTO>>(articles.OrderByDescending(a => a.like).Take(likeCount).ToList());
            });
        }

        /// <summary>
        /// 获取浏览数最多的文章
        /// </summary>
        /// <returns></returns>
        public async Task<List<ArticleDTO>> GetArticlesByVisitAsync(int visitCount)
        {
            return await Task.Run(() =>
            {
                var articles = ArticleRepository.GetModels(a => a.isShow);
                return Mapper.Map<List<ArticleDTO>>(articles.OrderByDescending(a => a.visits).Take(visitCount).ToList());
            });
        }

        /// <summary>
        /// 获取所有文章
        /// </summary>
        /// <returns></returns>
        public async Task<List<ArticleDTO>> GetAllArticlesAsync()
        {
            return await Task.Run(() =>
            {
                var articles =  ArticleRepository.GetModels(a => a.id != 0);
                return Mapper.Map<List<ArticleDTO>>(articles.OrderByDescending(a => a.createDate).ToList());
            });
        }

        /// <summary>
        /// 删除文章列表
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveArticleAsync(int tId)
        {
            var article = await ArticleRepository.GetSingleModelAsync(a => a.id == tId);
            return Repository.Delete(article);
        }

        /// <summary>
        /// 根据类型Id获取文章数量
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public Task<int> GetArticleCountByCategoryIdAsync(int categoryId)
        {
            return Task.FromResult(ArticleRepository.GetModelsCountByCondition(
                a => a.categoryId == categoryId && a.isShow));
        }
    }
}