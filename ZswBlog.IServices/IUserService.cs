
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface IUserService : IBaseService<UserEntity>
    {
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        Task<List<UserEntity>> GetAllUsersAsync();
        /// <summary>
        /// 获取最近登录的用户
        /// </summary>
        /// <param name="count">获取数量</param>
        /// <returns></returns>
        Task<List<UserEntity>> GetUsersNearVisit(int count);
    }
}
