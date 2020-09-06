
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface IUserService : IBaseService<User>
    {
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> GetUserByOpenIdAsync(string openId);
        Task<List<User>> GetAllUsersAsync();
        Task<List<User>> GetUsersNearVisit(int count);
    }
}
