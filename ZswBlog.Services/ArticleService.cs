
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;
using ZswBlog.Services;

namespace Services
{
    public class ArticleService : BaseService<ArticleEntity, IArticleRepository>, IArticleService
    {
        //private readonly IArticleRepository _repository;
        //public ArticleService(IArticleRepository repository)
        //{
        //    _repository = repository;
        //}
        //private ILog log = LogManager.GetLogger(typeof(ArticleService));

        public async Task<List<ArticleEntity>> GetArticlesByDimTitleAsync(string dimTitle)
        {

            return await Task.Run(() =>
            {
                List<ArticleEntity> articles = _repository.GetModels(a => a.title.Contains(dimTitle)).Where(a => a.isShow).ToList();
                return articles;
            });
        }

        public async Task<ArticleEntity> GetArticleByIdAsync(int articleId)
        {
            return await Task.Run(() =>
            {
                ArticleEntity article = _repository.GetSingleModel(a => a.id == articleId);
                return article;
            });
        }

        private static int pageCount;
        public async Task<List<ArticleEntity>> GetArticlesByPageAndIsShowAsync(int limit, int pageIndex, bool isShow)
        {
            return await Task.Run(() =>
            {
                IEnumerable<ArticleEntity> articles = _repository.GetModelsByPage(limit, pageIndex, false, a => a.createDate, a => a.id != 0, out pageCount);
                return isShow ? articles.Where(a => a.isShow).ToList() : articles.Where(a => !a.isShow).ToList();
            });
        }

        private static int pageClassCount;
        public async Task<List<ArticleEntity>> GetArticlesByPageClassAndIsShowAsync(int limit, int pageIndex, int articleClass, bool isShow)
        {
            return await Task.Run(() =>
            {
                IEnumerable<ArticleEntity> articles = _repository.GetModelsByPage(limit, pageIndex, false, a => a.createDate, a => a.categoryId == articleClass, out pageClassCount);
                return isShow ? articles.Where(a => a.isShow).ToList() : articles.Where(a => !a.isShow).ToList();
            });
        }

        public async Task<List<ArticleEntity>> GetArticlesByTop5LikeAsync()
        {
            return await Task.Run(() =>
            {
                List<ArticleEntity> articles = _repository.GetModels(a => a.isShow).OrderByDescending(a => a.like).Take(5).ToList();
                return articles;
            });
        }

        public async Task<List<ArticleEntity>> GetArticlesByTop5VisitAsync()
        {
            return await Task.Run(() =>
            {
                List<ArticleEntity> articles = _repository.GetModels(a => a.isShow).OrderByDescending(a => a.visits).Take(5).ToList();
                return articles;
            });
        }

        public async Task<List<ArticleEntity>> GetAllArticlesAsync()
        {
            return await Task.Run(() =>
            {
                List<ArticleEntity> articles = _repository.GetModels(a => a.id != 0).OrderByDescending(a => a.createDate).ToList();
                return articles;
            });
        }

        public async Task<List<int>> GetArticlesByAllClassType()
        {
            return await Task.Run(() =>
            {
                List<int> classTypes = _repository.GetModels(a => a.id != 0).Select(p => p.categoryId).Distinct().ToList();
                return classTypes;
            });
        }

        public async Task<bool> RemoveEntityAsync(int tId)
        {
            ArticleEntity article = await this.GetArticleByIdAsync(tId);
            article.isShow = false;
            return _repository.Update(article);
        }
    }
}
