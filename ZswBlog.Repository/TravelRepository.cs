using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class TravelRepository : BasicRepository<Travel>, ITravelRepository, IBaseRepository<Travel> { }
}
