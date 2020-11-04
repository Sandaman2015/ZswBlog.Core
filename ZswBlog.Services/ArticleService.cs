using Autofac.Extras.DynamicProxy;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class ArticleService : BaseService<ArticleEntity, IArticleRepository>, IArticleService
    {
        public IArticleRepository _articleRepository { get; set; }

        public ICategoryService _categoryService { get; set; }

        public IArticleTagService _articleTagService { get; set; }
        public IMapper _mapper { get; set; }

        /// <summary>
        /// 分页根据显示获取文章
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public PageDTO<ArticleDTO> GetArticleListByCategoryId(int limit, int pageIndex, int categoryId)
        {
            List<ArticleEntity> articles = _articleRepository.GetModelsByPage(limit, pageIndex, false, (ArticleEntity a) => a.visits, (ArticleEntity ac) => ac.categoryId == categoryId, out int pageCount).ToList();
            List<ArticleDTO> articleDTOList = _mapper.Map<List<ArticleDTO>>(articles);
            foreach (var articleDTO in articleDTOList)
            {
                articleDTO.category = _categoryService.GetCategoryById(articleDTO.categoryId);
                articleDTO.tags = _articleTagService.GetTagListByArticleId(articleDTO.id);
            }
            return new PageDTO<ArticleDTO>(limit,
                                           pageIndex,
                                           pageCount,
                                           articleDTOList);
        }

        /// <summary>
        /// 根据文章标题模糊查询
        /// </summary>
        /// <param name="dimTitle"></param>
        /// <returns></returns>
        public List<ArticleDTO> GetArticlesByDimTitle(string dimTitle)
        {
            List<ArticleEntity> articles = _articleRepository.GetModels(a => a.title.Contains(dimTitle)).Where(a => a.isShow).ToList();
            List<ArticleDTO> articleDTOList = _mapper.Map<List<ArticleDTO>>(articles);
            foreach (var articleDTO in articleDTOList)
            {
                articleDTO.category = _categoryService.GetCategoryById(articleDTO.categoryId);
                articleDTO.tags = _articleTagService.GetTagListByArticleId(articleDTO.id);
            }
            return articleDTOList;
        }

        /// <summary>
        /// 根据文章id获取文章DTO
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public ArticleDTO GetArticleById(int articleId)
        {
            ArticleDTO articleDTO = null;
            ArticleEntity article = _articleRepository.GetSingleModel(a => a.id == articleId);
            if (article == null)
            {
                return null;
            }
            articleDTO = _mapper.Map<ArticleDTO>(article);
            articleDTO.category = _categoryService.GetCategoryById(articleDTO.categoryId);
            articleDTO.tags = _articleTagService.GetTagListByArticleId(articleId);
            return articleDTO;
        }

        /// <summary>
        /// 分页获取文章DTO
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        public PageDTO<ArticleDTO> GetArticlesByPageAndIsShow(int limit, int pageIndex, bool isShow)
        {
            List<ArticleEntity> articles = _articleRepository.GetModelsByPage(limit, pageIndex, false, a => a.createDate, a => a.isShow == isShow, out int pageCount).ToList();
            List<ArticleDTO> articleDTOList = _mapper.Map<List<ArticleDTO>>(articles);
            foreach (var articleDTO in articleDTOList)
            {
                articleDTO.category = _categoryService.GetCategoryById(articleDTO.categoryId);
                articleDTO.tags = _articleTagService.GetTagListByArticleId(articleDTO.id);
            }
            return new PageDTO<ArticleDTO>(limit,
                                           pageIndex,
                                           pageCount,
                                           articleDTOList);
        }

        /// <summary>
        /// 获取最近发布的文章
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ArticleDTO> GetArticlesByNearSave(int count)
        {
            List<ArticleEntity> articles = _articleRepository.GetModels(a => a.isShow).OrderByDescending(a => a.createDate).Take(count).ToList();
            return _mapper.Map<List<ArticleDTO>>(articles);
        }

        /// <summary>
        /// 根据类型分页获取文章DTO列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="articleClass"></param>
        /// <returns></returns>
        public PageDTO<ArticleDTO> GetArticlesByPageClass(int limit, int pageIndex, int articleClass)
        {
            List<ArticleEntity> articles = _articleRepository.GetModelsByPage(limit, pageIndex, false, a => a.createDate, a => a.categoryId == articleClass, out int pageCount).ToList();
            List<ArticleDTO> articleDTOList = _mapper.Map<List<ArticleDTO>>(articles);
            foreach (var articleDTO in articleDTOList)
            {
                articleDTO.category = _categoryService.GetCategoryById(articleDTO.categoryId);
                articleDTO.tags = _articleTagService.GetTagListByArticleId(articleDTO.id);
            }
            return new PageDTO<ArticleDTO>(limit,
                                           pageIndex,
                                           pageCount,
                                           articleDTOList);
        }

        /// <summary>
        /// 获取最喜爱的文章列表
        /// </summary>
        /// <returns></returns>
        public List<ArticleDTO> GetArticlesByLike(int likeCount)
        {
            List<ArticleEntity> articles = _articleRepository.GetModels(a => a.isShow).OrderByDescending(a => a.like).Take(likeCount).ToList();
            return _mapper.Map<List<ArticleDTO>>(articles);
        }

        /// <summary>
        /// 获取浏览数最多的文章
        /// </summary>
        /// <returns></returns>
        public List<ArticleDTO> GetArticlesByVisit(int visitCount)
        {
            List<ArticleEntity> articles = _articleRepository.GetModels(a => a.isShow).OrderByDescending(a => a.visits).Take(visitCount).ToList();
            return _mapper.Map<List<ArticleDTO>>(articles);
        }

        /// <summary>
        /// 获取所有文章
        /// </summary>
        /// <returns></returns>
        public List<ArticleDTO> GetAllArticles()
        {
            List<ArticleEntity> articles = _articleRepository.GetModels(a => a.id != 0).OrderByDescending(a => a.createDate).ToList();
            return _mapper.Map<List<ArticleDTO>>(articles);
        }

        /// <summary>
        /// 删除文章列表
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public bool RemoveEntity(int tId)
        {
            ArticleEntity article = _articleRepository.GetSingleModel((ArticleEntity a) => a.id == tId);
            article.isShow = false;
            return _repository.Update(article);
        }

        /// <summary>
        /// 根据类型Id获取文章数量
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public int GetArticleCountByCategoryId(int categoryId)
        {
            return _articleRepository.GetModelsCountByCondition(a => a.categoryId == categoryId);
        }
    }
}
