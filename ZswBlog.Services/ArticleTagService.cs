using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using ZswBlog.DTO;
using ZswBlog.IRepository;
using ZswBlog.IServices;
using ZswBlog.Entity;
using Microsoft.EntityFrameworkCore;

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
                var articles = ArticleRepository.GetModels(
                        a => articleTags.Select(c => c.articleId).ToList().Contains(a.id));
                var articleDtOs = Mapper.Map<List<ArticleDTO>>(articles.ToList());
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
            var articleTags =  ArticleTagRepository.GetModels(a => a.articleId == articleId);
            List<ArticleTagEntity> articleTagEntities =  articleTags.ToList();
            List<int> ids =  articleTagEntities.Select(e => e.tagId).ToList();
            var tags =  TagRepository.GetModels(a => ids.Contains(a.id));
            List<TagEntity> tagEntitys = await tags.ToListAsync();
            return Mapper.Map<List<TagDTO>>(tagEntitys);;
        }

        /// <summary>
        /// 删除所有已经绑定的文章标签
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAlreadyExistArticleTagAsync(int articleId)
        {
            var articleTags = ArticleTagRepository.GetModels(a => a.articleId == articleId);
            foreach (var item in articleTags.ToList())
            {
                await ArticleTagRepository.DeleteAsync(item);
            }
            return true;
        }
    }
}