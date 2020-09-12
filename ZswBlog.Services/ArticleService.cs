
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace Services
{
    public class ArticleService : BaseService, IArticleService
    {
        private readonly IArticleRepository _repository;
        public ArticleService(IArticleRepository repository)
        {
            _repository = repository;
        }
        //private ILog log = LogManager.GetLogger(typeof(ArticleService));

        public async Task<List<Article>> GetArticlesByDimTitleAsync(string dimTitle)
        {

            return await Task.Run(() =>
            {
                List<Article> articles = _repository.GetModels(a => a.ArticleTitle.Contains(dimTitle)).Where(a => a.IsShow).ToList();
                return articles;
            });
        }

        public async Task<Article> GetArticleByIdAsync(int articleId)
        {
            return await Task.Run(() =>
            {
                Article article = _repository.GetSingleModel(a => a.ArticleId == articleId);
                return article;
            });
        }

        private static int pageCount;
        public async Task<List<Article>> GetArticlesByPageAndIsShowAsync(int limit, int pageIndex, bool isShow)
        {
            return await Task.Run(() =>
            {
                IEnumerable<Article> articles = _repository.GetModelsByPage(limit, pageIndex, false, a => a.ArticleCreateTime, a => a.ArticleId != 0, out pageCount);
                return isShow ? articles.Where(a => a.IsShow).ToList() : articles.Where(a => !a.IsShow).ToList();
            });
        }

        private static int pageClassCount;
        public async Task<List<Article>> GetArticlesByPageClassAndIsShowAsync(int limit, int pageIndex, string articleClass, bool isShow)
        {
            return await Task.Run(() =>
            {
                IEnumerable<Article> articles = _repository.GetModelsByPage(limit, pageIndex, false, a => a.ArticleCreateTime, a => a.ArticleClass == articleClass, out pageClassCount);
                return isShow ? articles.Where(a => a.IsShow).ToList() : articles.Where(a => !a.IsShow).ToList();
            });
        }

        public async Task<List<Article>> GetArticlesByTop5LikeAsync()
        {
            return await Task.Run(() =>
            {
                List<Article> articles = _repository.GetModels(a => a.IsShow).OrderByDescending(a => a.ArticleLikes).Take(5).ToList();
                return articles;
            });
        }

        public async Task<List<Article>> GetArticlesByTop5VisitAsync()
        {
            return await Task.Run(() =>
            {
                List<Article> articles = _repository.GetModels(a => a.IsShow).OrderByDescending(a => a.ArticleVisits).Take(5).ToList();
                return articles;
            });
        }

        public async Task<int> pageTotalAsync()
        {
            return await Task.Run(() =>
            {
                return pageCount;
            });
        }

        public async Task<int> pageClassTotalAsync()
        {
            return await Task.Run(() =>
            {
                return pageClassCount;
            });
        }

        public async Task<List<Article>> GetAllArticlesAsync()
        {
            return await Task.Run(() =>
            {
                List<Article> articles = _repository.GetModels(a => a.ArticleId != 0).OrderByDescending(a => a.ArticleCreateTime).ToList();
                return articles;
            });
        }

        public async Task<List<string>> GetArticlesByAllClassType()
        {
            return await Task.Run(() =>
            {
                List<string> classTypes = _repository.GetModels(a => a.ArticleId != 0).Select(p => p.ArticleClass).Distinct().ToList();
                return classTypes;
            });
        }

        public async Task<bool> AddEntityAsync(Article t)
        {
            return await Task.Run(() =>
            {
                t.ArticleCreateTime = DateTime.Now;
                t.ArticleLikes = 0;
                t.ArticleVisits = 0;
                t.IsShow = true;
                return _repository.Add(t);
            });
        }

        public async Task<bool> RemoveEntityAsync(int tId)
        {
            Article article = await this.GetArticleByIdAsync(tId);
            return _repository.Add(article);
        }

        public async Task<bool> AlterEntityAsync(Article t)
        {
            return await Task.Run(() =>
            {
                return _repository.Update(t);
            });
        }
    }
}
