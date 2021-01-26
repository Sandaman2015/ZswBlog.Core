using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class FriendLinkRepository : BaseRepository<FriendLinkEntity>, IFriendLinkRepository, IBaseRepository<FriendLinkEntity>
    {

    }
}
