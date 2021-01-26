using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class ArticleTagRepository : BaseRepository<ArticleTagEntity>, IArticleTagRepository, IBaseRepository<ArticleTagEntity> { }
}
