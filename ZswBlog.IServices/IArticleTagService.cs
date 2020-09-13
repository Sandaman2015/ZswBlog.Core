using System.Collections.Generic;
using System.Threading.Tasks;
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
        Task<List<TagEntity>> GetTagsIdByArticleId(int articleId);

        /// <summary>
        /// 通过标签号获取所有属于他的文章
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        Task<List<ArticleEntity>> GetArticleListIdByTagId(int tagId);        
    }
}
