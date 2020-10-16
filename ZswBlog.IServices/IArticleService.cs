using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface IArticleService : IBaseService<ArticleEntity>
    {
        /// <summary>
        /// 根据类型获取分页文章
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="articleClass">文章类型</param>
        /// <returns></returns>
        PageDTO<ArticleDTO> GetArticlesByPageClass(int limit, int pageIndex, int articleClass);
        /// <summary>
        /// 根据文章标题模糊获取
        /// </summary>
        /// <param name="dimTitle">模糊的文章标题</param>
        /// <returns></returns>
        List<ArticleDTO> GetArticlesByDimTitle(string dimTitle);
        /// <summary>
        /// 可根据是否显示获取分页文章
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="isShow">选择是否显示</param>
        /// <returns></returns>
        PageDTO<ArticleDTO> GetArticlesByPageAndIsShow(int limit, int pageIndex, bool isShow);
        /// <summary>
        /// 根据文章Id号获取文章
        /// </summary>
        /// <param name="articleId">文章Id</param>
        /// <returns></returns>
        ArticleDTO GetArticleById(int articleId);
        /// <summary>
        /// 获取所有文章
        /// </summary>
        /// <returns></returns>
        List<ArticleDTO> GetAllArticles();
        /// <summary>
        /// 获取满意度最高的文章
        /// </summary>
        /// <returns></returns>
        List<ArticleDTO> GetArticlesByLike(int likeCount);
        /// <summary>
        /// 获取浏览度最高的文章
        /// </summary>
        /// <returns></returns>
        List<ArticleDTO> GetArticlesByVisit(int visitCount);
        /// <summary>
        /// 根据文章分类获取文章列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        PageDTO<ArticleDTO> GetArticleListByCategoryId(int limit, int pageIndex, int categoryId);

        /// <summary>
        /// 根据类型获取文章条数
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        int GetArticleCountByCategoryId(int categoryId);
    }
}
