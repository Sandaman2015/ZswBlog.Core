using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class ArticleRepository : BaseRepository<ArticleEntity>, IArticleRepository, IBaseRepository<ArticleEntity>
    {

    }
}
