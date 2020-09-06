using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class TagRepository : BasicRepository<Tag>, ITagRepository, IBaseRepository<Tag> { }
}
