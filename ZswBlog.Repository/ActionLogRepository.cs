using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class ActionLogRepository: BaseRepository<ActionLogEntity>, IActionLogRepository, IBaseRepository<ActionLogEntity>
    {
        
    }
}