using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class TravelRepository : BaseRepository<TravelEntity>, ITravelRepository, IBaseRepository<TravelEntity> { }
}
