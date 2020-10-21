using System.Collections.Generic;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IRepository
{
    public interface IArticleRepository : IBaseRepository<ArticleEntity> {
        List<ArticleDTO> GetArticlesByNearSave(int count);
    }
}
