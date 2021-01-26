using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class ArticleRepository : BaseRepository<ArticleEntity>, IArticleRepository, IBaseRepository<ArticleEntity>
    {
    }
}
