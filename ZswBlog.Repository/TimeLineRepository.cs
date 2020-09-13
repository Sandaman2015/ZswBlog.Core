using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class TimeLineRepository : BaseRepository<TimeLineEntity>, ITimeLineRepository, IBaseRepository<TimeLineEntity> { }
}
