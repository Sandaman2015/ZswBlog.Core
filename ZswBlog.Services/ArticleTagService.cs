
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class ArticleTagService : BaseService<ArticleTagEntity, IArticleTagRepository>, IArticleTagService
    {
        public IArticleTagRepository _articleTagRepository { get; set; }
        public IArticleRepository _articleRepository { get; set; }
        public ITagRepository _tagRepository { get; set; }
        public IMapper _mapper { get; set; }

        //public ArticleTagService(IArticleTagRepository repository, IArticleRepository articleRepository, ITagRepository tagRepository, IMapper mapper)
        //{
        //    _repository = repository;
        //    _articleRepository = articleRepository;
        //    _tagRepository = tagRepository;
        //    _mapper = mapper;
        //}

        /// <summary>
        /// 根据标签id获取
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public PageDTO<ArticleDTO> GetArticleListIdByTagId(int limit, int pageIndex, int tagId)
        {
            int pageCount;
            List<ArticleEntity> articles = new List<ArticleEntity>();
            List<ArticleTagEntity> articleTags = _articleTagRepository.GetModelsByPage(limit, pageIndex, false, (ArticleTagEntity a) => a.id, a => a.id == tagId, out pageCount).ToList();
            foreach (var item in articleTags)
            {
                ArticleEntity article = _articleRepository.GetSingleModel(a => a.id == item.articleId);
                articles.Add(article);
            }
            List<ArticleDTO> articleDTOs = _mapper.Map<List<ArticleDTO>>(articles);
            return new PageDTO<ArticleDTO>(pageIndex, limit, pageCount, articleDTOs);
        }

        /// <summary>
        /// 根据文章id获取所有标签
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public List<TagDTO> GetTagListByArticleId(int articleId)
        {
            List<TagEntity> tags = new List<TagEntity>();
            List<ArticleTagEntity> articleTags = _articleTagRepository.GetModels(a => a.articleId == articleId).ToList();
            foreach (var item in articleTags)
            {
                TagEntity tag = _tagRepository.GetSingleModel(a => a.id == item.tagId);
                tags.Add(tag);
            }
            List<TagDTO> tagDTOs = _mapper.Map<List<TagDTO>>(tags);
            return tagDTOs;
        }
        
        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public bool RemoveEntityAsync(int tId)
        {
            ArticleTagEntity entity = _articleTagRepository.GetSingleModel((ArticleTagEntity at) => at.id == tId);
            return _articleTagRepository.Delete(entity);
        }
    }
}
