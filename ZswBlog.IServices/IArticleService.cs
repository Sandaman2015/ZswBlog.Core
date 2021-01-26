using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;

namespace ZswBlog.IServices
{
    public interface IArticleService : IBaseService<ArticleEntity>
    {
        /// <summary>
        /// 根据文章标题模糊获取
        /// </summary>
        /// <param name="dimTitle">模糊的文章标题</param>
        /// <returns></returns>
        Task<List<ArticleDTO>> GetArticlesByDimTitleAsync(string dimTitle);
        /// <summary>
        /// 可根据是否显示获取分页文章
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="isShow">选择是否显示</param>
        /// <returns></returns>
        Task<PageDTO<ArticleDTO>> GetArticlesByPageAndIsShowAsync(int limit, int pageIndex, bool isShow);
        /// <summary>
        /// 根据文章Id号获取文章
        /// </summary>
        /// <param name="articleId">文章Id</param>
        /// <param name="isShow">是否显示</param>
        /// <returns></returns>
        Task<ArticleDTO> GetArticleByIdAsync(int articleId, bool isShow);
        /// <summary>
        /// 获取所有文章
        /// </summary>
        /// <returns></returns>
        Task<List<ArticleDTO>> GetAllArticlesAsync();
        /// <summary>
        /// 获取满意度最高的文章
        /// </summary>
        /// <returns></returns>
        Task<List<ArticleDTO>> GetArticlesByLikeAsync(int likeCount);
        /// <summary>
        /// 获取浏览度最高的文章
        /// </summary>
        /// <returns></returns>
        Task<List<ArticleDTO>> GetArticlesByVisitAsync(int visitCount);
        /// <summary>
        /// 根据文章分类获取文章列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<PageDTO<ArticleDTO>> GetArticleListByCategoryIdAsync(int limit, int pageIndex, int categoryId);

        /// <summary>
        /// 根据类型获取文章条数
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<int> GetArticleCountByCategoryIdAsync(int categoryId);
        /// <summary>
        /// 获取最近发布的文章
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<ArticleDTO>> GetArticlesByNearSaveAsync(int count);
        /// <summary>
        /// 添加文章喜爱数
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<bool> AddArticleLikeAsync(int articleId);
    }
}
