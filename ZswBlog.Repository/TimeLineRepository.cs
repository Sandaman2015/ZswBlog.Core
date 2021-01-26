using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class TimeLineRepository : BaseRepository<TimeLineEntity>, ITimeLineRepository, IBaseRepository<TimeLineEntity> { }
}
