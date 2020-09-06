using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface IArticleService : IBaseService<Article>
    {
        /// <summary>
        /// 获取分页数据后的文章总数
        /// </summary>
        /// <returns></returns>
        Task<int> pageTotalAsync();
        /// <summary>
        /// 获取根据类型分页后的文章总数
        /// </summary>
        /// <returns></returns>
        Task<int> pageClassTotalAsync();
        /// <summary>
        /// 可根据是否显示和类型获取分页文章
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="articleClass">文章类型</param>
        /// <param name="isShow">选择是否显示</param>
        /// <returns></returns>
        Task<List<Article>> GetArticlesByPageClassAndIsShowAsync(int limit, int pageIndex, string articleClass, bool isShow);
        /// <summary>
        /// 根据文章标题模糊获取
        /// </summary>
        /// <param name="dimTitle">模糊的文章标题</param>
        /// <returns></returns>
        Task<List<Article>> GetArticlesByDimTitleAsync(string dimTitle);
        /// <summary>
        /// 可根据是否显示获取分页文章
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="isShow">选择是否显示</param>
        /// <returns></returns>
        Task<List<Article>> GetArticlesByPageAndIsShowAsync(int limit, int pageIndex, bool isShow);
        /// <summary>
        /// 根据文章Id号获取文章
        /// </summary>
        /// <param name="articleId">文章Id</param>
        /// <returns></returns>
        Task<Article> GetArticleByIdAsync(int articleId);
        /// <summary>
        /// 获取所有文章
        /// </summary>
        /// <returns></returns>
        Task<List<Article>> GetAllArticlesAsync();
        /// <summary>
        /// 获取前五个满意度最高的文章
        /// </summary>
        /// <returns></returns>
        Task<List<Article>> GetArticlesByTop5LikeAsync();
        /// <summary>
        /// 获取前五个浏览度最高的文章
        /// </summary>
        /// <returns></returns>
        Task<List<Article>> GetArticlesByTop5VisitAsync();
        /// <summary>
        /// 获取所有文章类别
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetArticlesByAllClassType();
    }
}
