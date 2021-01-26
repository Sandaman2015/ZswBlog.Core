using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class TagRepository : BaseRepository<TagEntity>, ITagRepository, IBaseRepository<TagEntity> { }
}
