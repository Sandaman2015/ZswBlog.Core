using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class SiteTagRepository : BaseRepository<SiteTagEntity>, ISiteTagRepository, IBaseRepository<SiteTagEntity>
    {

    }
}
