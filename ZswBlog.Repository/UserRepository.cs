using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class UserRepository : BasicRepository<User>, IUserRepository, IBaseRepository<User>
    {

    }
}
