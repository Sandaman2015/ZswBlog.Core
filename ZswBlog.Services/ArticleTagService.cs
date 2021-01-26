﻿using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class ArticleTagService : BaseService<ArticleTagEntity, IArticleTagRepository>, IArticleTagService
    {
        public IArticleTagRepository ArticleTagRepository { get; set; }
        public IArticleRepository ArticleRepository { get; set; }
        public ITagRepository TagRepository { get; set; }
        public IMapper Mapper { get; set; }

        /// <summary>
        /// 根据标签id获取
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public async Task<PageDTO<ArticleDTO>> GetArticleListIdByTagIdAsync(int limit, int pageIndex, int tagId)
        {
            return await Task.Run(() =>
            {
                var articleTags = ArticleTagRepository.GetModelsByPage(limit, pageIndex, false,
                    a => a.id, a => a.id == tagId, out var pageCount);
                var articles = articleTags
                    .Select(item => ArticleRepository.GetSingleModelAsync(a => a.id == item.articleId))
                    .Select(article => article.Result).ToList();
                var articleDtOs = Mapper.Map<List<ArticleDTO>>(articles);
                return new PageDTO<ArticleDTO>(pageIndex, limit, pageCount, articleDtOs);
            });
        }

        /// <summary>
        /// 根据文章id获取所有标签
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<List<TagDTO>> GetTagListByArticleIdAsync(int articleId)
        {
            var articleTags =
                await ArticleTagRepository.GetModelsAsync(a => a.articleId == articleId);
            List<TagEntity> tags = new List<TagEntity>();
            foreach (var item in articleTags.ToList())
            {
                var tag = await TagRepository.GetSingleModelAsync(a => a.id == item.tagId);
                tags.Add(tag);
            }
            var tagDtOs = Mapper.Map<List<TagDTO>>(tags);
            return tagDtOs;
        }

        /// <summary>
        /// 删除所有已经绑定的文章标签
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAlreadyExistArticleTagAsync(int articleId)
        {
            var articleTags =
              await ArticleTagRepository.GetModelsAsync(a => a.articleId == articleId);
            articleTags.ToList().ForEach(a => { ArticleTagRepository.DeleteAsync(a); });
            return true;
        }

        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveEntityAsync(int tId)
        {
            var entity =
                await ArticleTagRepository.GetSingleModelAsync(at => at.id == tId);
            return await ArticleTagRepository.DeleteAsync(entity);
        }
    }
}