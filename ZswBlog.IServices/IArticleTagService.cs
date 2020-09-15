using System.Collections.Generic;
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
        List<TagDTO> GetTagListByArticleId(int articleId);

        /// <summary>
        /// 通过标签号分页获取所有属于他的文章
        /// </summary>
        /// <param name="limit">页码</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="tagId">标签id</param>
        /// <returns></returns>
        PageDTO<ArticleDTO> GetArticleListIdByTagId(int limit, int pageIndex, int tagId);
    }
}
