using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class ArticleRepository : BasicRepository<Article>, IArticleRepository, IBaseRepository<Article>
    {

    }
}
