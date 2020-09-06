using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class ArticleTagRepository : BasicRepository<ArticleTag>, IArticleTagRepository, IBaseRepository<ArticleTag> { }
}
