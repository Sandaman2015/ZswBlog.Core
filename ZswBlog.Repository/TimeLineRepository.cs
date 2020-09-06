using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class TimeLineRepository : BasicRepository<Timeline>, ITimeLineRepository, IBaseRepository<Timeline> { }
}
