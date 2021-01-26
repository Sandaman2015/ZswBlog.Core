using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class TravelRepository : BaseRepository<TravelEntity>, ITravelRepository, IBaseRepository<TravelEntity> { }
}
