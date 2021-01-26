using ZswBlog.Entity.DbContext;

namespace ZswBlog.IRepository
{
    //public interface IUserRepository : IBaseRepository<User> { }
    public interface IUserRepository : IBaseRepository<UserEntity> { }
}
