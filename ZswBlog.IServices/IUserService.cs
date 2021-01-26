
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;

namespace ZswBlog.IServices
{
    public interface IUserService : IBaseService<UserEntity>
    {
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        Task<List<UserDTO>> GetAllUsersAsync();
        /// <summary>
        /// 获取最近登录的用户
        /// </summary>
        /// <param name="count">获取数量</param>
        /// <returns></returns>
        Task<List<UserDTO>> GetUsersNearVisitAsync(int count);

        /// <summary>
        /// 根据id获取用户信息
        /// </summary>
        /// <param name="id">用户信息</param>
        /// <returns></returns>
        Task<UserDTO> GetUserByIdAsync(int id);

        /// <summary>
        /// 根据条件获取用户
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<UserEntity> GetUserByConditionAsync(Expression<Func<UserEntity, bool>> whereLambda);

        /// <summary>
        /// 验证用户名和密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<UserEntity> ValidatePasswordAsync(string userName, string password);
    }
}
