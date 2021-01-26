using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Common.Util;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class ArticleService : BaseService<ArticleEntity, IArticleRepository>, IArticleService
    {
        public IArticleRepository ArticleRepository { get; set; }
        public ICategoryService CategoryService { get; set; }
        public IArticleTagService ArticleTagService { get; set; }
        public IMapper Mapper { get; set; }

        /// <summary>
        /// 分页根据显示获取文章
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<PageDTO<ArticleDTO>> GetArticleListByCategoryIdAsync(int limit, int pageIndex, int categoryId)
        {
            return await Task.Run(() =>
            {
                var articles = ArticleRepository.GetModelsByPage(limit, pageIndex, false,
                    a => a.visits, ac => ac.categoryId == categoryId && ac.isShow,
                    out var pageCount).ToList();
                var articleDtoList = Mapper.Map<List<ArticleDTO>>(articles);
                foreach (var articleDto in articleDtoList)
                {
                    articleDto.category = CategoryService.GetCategoryByIdAsync(articleDto.categoryId).Result;
                    articleDto.tags = ArticleTagService.GetTagListByArticleIdAsync(articleDto.id).Result;
                    articleDto.content = StringHelper.ReplaceTag(articleDto.content, 500);
                }

                return new PageDTO<ArticleDTO>(limit,
                    pageIndex,
                    pageCount,
                    articleDtoList);
            });
        }

        /// <summary>
        /// 根据文章标题模糊查询
        /// </summary>
        /// <param name="dimTitle"></param>
        /// <returns></returns>
        public async Task<List<ArticleDTO>> GetArticlesByDimTitleAsync(string dimTitle)
        {
            return await Task.Run(() =>
            {
                var articles = ArticleRepository.GetModelsAsync(a => a.title.Contains(dimTitle))
                    .Result.Where(a => a.isShow).ToList();
                var articleDtoList = Mapper.Map<List<ArticleDTO>>(articles);
                foreach (var articleDto in articleDtoList)
                {
                    articleDto.category = CategoryService.GetCategoryByIdAsync(articleDto.categoryId).Result;
                    articleDto.tags = ArticleTagService.GetTagListByArticleIdAsync(articleDto.id).Result;
                    articleDto.content = StringHelper.ReplaceTag(articleDto.content, 500);
                }

                return articleDtoList;
            });
        }

        /// <summary>
        /// 根据文章id获取文章DTO
        /// </summary>
        /// <param name="articleId">文章编码</param>
        /// <param name="isShow">是否显示</param>
        /// <returns></returns>
        public async Task<ArticleDTO> GetArticleByIdAsync(int articleId, bool isShow)
        {
            return await Task.Run(() =>
            {
                var article = isShow
                    ? ArticleRepository.GetSingleModelAsync(a => a.id == articleId && a.isShow).Result
                    : ArticleRepository.GetSingleModelAsync(a => a.id == articleId).Result;
                if (article == null)
                {
                    throw new Exception("未找到文章");
                }

                var articleDto = Mapper.Map<ArticleDTO>(article);
                articleDto.category = CategoryService.GetCategoryByIdAsync(articleDto.categoryId).Result;
                articleDto.tags = ArticleTagService.GetTagListByArticleIdAsync(articleId).Result;
                AddArticleVisitAsync(articleId);
                return articleDto;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="articleId"></param>
        private void AddArticleVisitAsync(int articleId)
        {
            var article = ArticleRepository.GetSingleModelAsync(a => a.id == articleId);
            article.Result.visits += 1;
            ArticleRepository.UpdateAsync(article.Result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<bool> AddArticleLikeAsync(int articleId)
        {
            return await Task.Run(() =>
            {
                var article = ArticleRepository.GetSingleModelAsync(a => a.id == articleId);
                article.Result.like += 1;
                return ArticleRepository.UpdateAsync(article.Result);
            });
        }

        /// <summary>
        /// 分页获取文章DTO
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        public async Task<PageDTO<ArticleDTO>> GetArticlesByPageAndIsShowAsync(int limit, int pageIndex, bool isShow)
        {
            return await Task.Run(() =>
            {
                List<ArticleEntity> articles;
                int pageCount;
                if (isShow)
                {
                    articles = ArticleRepository
                        .GetModelsByPage(limit, pageIndex, false, a => a.createDate, a => a.isShow, out pageCount)
                        .ToList();
                }
                else
                {
                    articles = ArticleRepository
                        .GetModelsByPage(limit, pageIndex, false, a => a.createDate, a => a.id != 0, out pageCount)
                        .ToList();
                }

                var articleDtoList = Mapper.Map<List<ArticleDTO>>(articles);
                foreach (var articleDto in articleDtoList)
                {
                    articleDto.category = CategoryService.GetCategoryByIdAsync(articleDto.categoryId).Result;
                    articleDto.tags = ArticleTagService.GetTagListByArticleIdAsync(articleDto.id).Result;
                    articleDto.content = StringHelper.ReplaceTag(articleDto.content, 500);
                }

                return new PageDTO<ArticleDTO>(limit,
                    pageIndex,
                    pageCount,
                    articleDtoList);
            });
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
                var articles = ArticleRepository.GetModelsAsync(a => a.isShow)
                    .Result.OrderByDescending(a => a.createDate).Take(count).ToList();
                var articleDtoList = Mapper.Map<List<ArticleDTO>>(articles);
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
                var articles = ArticleRepository.GetModelsAsync(a => a.isShow)
                    .Result.OrderByDescending(a => a.like).Take(likeCount).ToList();
                return Mapper.Map<List<ArticleDTO>>(articles);
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
                var articles = ArticleRepository.GetModelsAsync(a => a.isShow)
                    .Result.OrderByDescending(a => a.visits).Take(visitCount).ToList();
                return Mapper.Map<List<ArticleDTO>>(articles);
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
                var articles = ArticleRepository.GetModelsAsync(a => a.id != 0)
                    .Result.OrderByDescending(a => a.createDate).ToList();
                return Mapper.Map<List<ArticleDTO>>(articles);
            });
        }

        /// <summary>
        /// 删除文章列表
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveEntity(int tId)
        {
            return await Task.Run(() =>
            {
                var article = ArticleRepository.GetSingleModelAsync( a => a.id == tId);
                article.Result.isShow = false;
                return Repository.UpdateAsync(article.Result);
            });
        }

        /// <summary>
        /// 根据类型Id获取文章数量
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<int> GetArticleCountByCategoryIdAsync(int categoryId)
        {
            return await Task.Run(() =>
            {
                return ArticleRepository.GetModelsCountByConditionAsync(
                    a => a.categoryId == categoryId && a.isShow);
            });
        }
    }
}