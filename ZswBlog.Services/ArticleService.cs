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
            var articles = ArticleRepository.GetModelsByPage(limit, pageIndex, false,
                a => a.visits, ac => ac.categoryId == categoryId && ac.isShow,
                out var pageCount).ToList();
            var articleDtoList = Mapper.Map<List<ArticleDTO>>(articles);
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
        /// <returns></returns>
        public async Task<List<ArticleDTO>> GetArticlesByDimTitleAsync(string dimTitle)
        {
            var articles = await ArticleRepository.GetModelsAsync(a => a.title.Contains(dimTitle));
            var articleDtoList = Mapper.Map<List<ArticleDTO>>(articles.Where(a => a.isShow));
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
        /// <returns></returns>
        public async Task<ArticleDTO> GetArticleByIdAsync(int articleId, bool isShow)
        {
            var article = isShow
                ? await ArticleRepository.GetSingleModelAsync(a => a.id == articleId && a.isShow)
                : await ArticleRepository.GetSingleModelAsync(a => a.id == articleId);
            if (article == null)
            {
                throw new Exception("未找到文章");
            }

            var articleDto = Mapper.Map<ArticleDTO>(article);
            articleDto.tags = await ArticleTagService.GetTagListByArticleIdAsync(articleId);
            AddArticleVisitAsync(articleId);
            return articleDto;
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
            var article = await ArticleRepository.GetSingleModelAsync(a => a.id == articleId);
            article.like += 1;
            return await ArticleRepository.UpdateAsync(article);
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
                articleDto.tags = (await ArticleTagService.GetTagListByArticleIdAsync(articleDto.id)).ToList();
                articleDto.content = StringHelper.ReplaceTag(articleDto.content, 500);
            }

            return new PageDTO<ArticleDTO>(limit,
                pageIndex,
                pageCount,
                articleDtoList);
        }

        /// <summary>
        /// 获取最近发布的文章
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<List<ArticleDTO>> GetArticlesByNearSaveAsync(int count)
        {
            var articles = await ArticleRepository.GetModelsAsync(a => a.isShow);
            var articleDtoList = Mapper.Map<List<ArticleDTO>>(articles.OrderByDescending(a => a.createDate).Take(count).ToList());
            foreach (var articleDto in articleDtoList)
            {
                articleDto.content = StringHelper.ReplaceTag(articleDto.content, 500);
            }

            return articleDtoList;
        }

        /// <summary>
        /// 获取最喜爱的文章列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ArticleDTO>> GetArticlesByLikeAsync(int likeCount)
        {
            var articles = await ArticleRepository.GetModelsAsync(a => a.isShow);
            return Mapper.Map<List<ArticleDTO>>(articles.OrderByDescending(a => a.like).Take(likeCount).ToList());
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
            var articles = await ArticleRepository.GetModelsAsync(a => a.id != 0);
            return Mapper.Map<List<ArticleDTO>>(articles.OrderByDescending(a => a.createDate).ToList());
        }

        /// <summary>
        /// 删除文章列表
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveEntity(int tId)
        {
            var article = await ArticleRepository.GetSingleModelAsync(a => a.id == tId);
            article.isShow = false;
            return await Repository.UpdateAsync(article);
        }

        /// <summary>
        /// 根据类型Id获取文章数量
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<int> GetArticleCountByCategoryIdAsync(int categoryId)
        {
            return await ArticleRepository.GetModelsCountByConditionAsync(
                a => a.categoryId == categoryId && a.isShow);
        }
    }
}