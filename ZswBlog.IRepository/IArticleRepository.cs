using System.Collections.Generic;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;

namespace ZswBlog.IRepository
{
    public interface IArticleRepository : IBaseRepository<ArticleEntity> {
    }
}
