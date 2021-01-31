using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class ArticleTagRepository : BaseRepository<ArticleTagEntity>, IArticleTagRepository, IBaseRepository<ArticleTagEntity> { }
}
