using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface IArticleTagService : IBaseService<ArticleTagEntity>
    {
        /// <summary>
        /// 根据文章号获取该文章的所有标签
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<List<TagDTO>> GetTagListByArticleIdAsync(int articleId);

        /// <summary>
        /// 通过标签号分页获取所有属于他的文章
        /// </summary>
        /// <param name="limit">页码</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="tagId">标签id</param>
        /// <returns></returns>
        Task<PageDTO<ArticleDTO>> GetArticleListIdByTagIdAsync(int limit, int pageIndex, int tagId);

        /// <summary>
        /// 删除所有已经绑定的文章标签
        /// </summary>
        /// <param name="articleId">文章编号</param>
        /// <returns></returns>
        Task<bool> RemoveAlreadyExistArticleTagAsync(int articleId);
    }
}
