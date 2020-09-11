
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;
using ZswBlog.Util;

namespace Services
{
    public class ArticleTagService : BaseService, IArticleTagService
    {
        public ArticleTagService(IArticleTagRepository repository, IArticleRepository articleRepository, ITagRepository tagRepository)
        {
            _repository = repository;
            _articleRepository = articleRepository;
            _tagRepository = tagRepository;
        }
        private readonly IArticleTagRepository _repository;
        private readonly IArticleRepository _articleRepository;
        private readonly ITagRepository _tagRepository;
        public async Task<List<Article>> GetArticlesIdByTagId(int tagId)
        {
            return await Task.Run(() =>
            {
                List<Article> articles = new List<Article>();
                List<ArticleTag> articleTags = _repository.GetModels(a => a.TagId == tagId).ToList();
                foreach (var item in articleTags)
                {
                    Article article = _articleRepository.GetSingleModel(a => a.ArticleId == item.ArticleId);
                    article.ArticleContent = StringHelper.StringTruncat(article.ArticleContent, 10, "...", out int length).Substring(0, 25);
                    articles.Add(article);
                }
                return articles;
            });
        }

        public async Task<List<List<Article>>> GetArticlesIdByTagIds(List<int> tagId)
        {
            return await Task.Run(() =>
            {
                List<List<Article>> articleLastList = new List<List<Article>>();//定义总列表
                foreach (var item in tagId.ToList())//遍历所有的tagid
                {
                    //通过tagid获取到所有的文章标签表
                    List<ArticleTag> articleTags = _repository.GetModels(a => a.TagId == item).ToList();
                    List<Article> articles = new List<Article>();
                    foreach (var that in articleTags)//遍历所有的文章标签表
                    {
                        Article article = _articleRepository.GetSingleModel(a => a.ArticleId == that.ArticleId);
                        article.ArticleContent = StringHelper.StringTruncat(article.ArticleContent, 10, "...", out int length).Substring(0, 25) + "...";
                        articles.Add(article);
                    }
                    articleLastList.Add(articles);
                }
                return articleLastList;
            });
        }

        public async Task<List<Tag>> GetTagsIdByArticleId(int articleId)
        {
            return await Task.Run(() =>
            {
                List<Tag> tags = new List<Tag>();
                List<ArticleTag> articleTags = _repository.GetModels(a => a.ArticleId == articleId).ToList();
                foreach (var item in articleTags)
                {
                    Tag tag = _tagRepository.GetSingleModel(a => a.TagId == item.TagId);
                    tags.Add(tag);
                }
                return tags as List<Tag>;
            });
        }

        Task<bool> IBaseService<ArticleTag>.AddEntityAsync(ArticleTag t)
        {
            throw new NotImplementedException();
        }

        Task<bool> IBaseService<ArticleTag>.RemoveEntityAsync(int tId)
        {
            throw new NotImplementedException();
        }

        Task<bool> IBaseService<ArticleTag>.AlterEntityAsync(ArticleTag t)
        {
            throw new NotImplementedException();
        }

    }
}
