
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface IUserService : IBaseService<UserEntity>
    {
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        List<UserDTO> GetAllUsers();
        /// <summary>
        /// 获取最近登录的用户
        /// </summary>
        /// <param name="count">获取数量</param>
        /// <returns></returns>
        List<UserDTO> GetUsersNearVisit(int count);

        /// <summary>
        /// 根据id获取用户信息
        /// </summary>
        /// <param name="id">用户信息</param>
        /// <returns></returns>
        UserDTO GetUserById(int id);
    }
}
