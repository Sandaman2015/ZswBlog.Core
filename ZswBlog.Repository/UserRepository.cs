using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository, IBaseRepository<UserEntity>
    {

    }
}
