using Autofac.Extras.DynamicProxy;
using AutoMapper;
using NETCore.Encrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZswBlog.Common.AopConfig;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;
using ZswBlog.IServices;
using ZswBlog.Query;
using ZswBlog.ThirdParty;

namespace ZswBlog.Services
{
    // [Intercept(typeof(EnableTransaction))]
    public class UserService : BaseService<UserEntity, IUserRepository>, IUserService
    {
        public IUserRepository UserRepository { get; set; }
        public IMapper Mapper { get; set; }
        public IQQUserInfoService UserQqInfoService { get; set; }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await UserRepository.GetModelsAsync(a => a.id != 0);
            return Mapper.Map<List<UserDTO>>(users.ToList());
        }

        public async Task<UserDTO> GetUserByOpenIdAsync(string openId)
        {
            var infoEntity = await UserQqInfoService.GetQQUserInfoByOpenIdAsync(openId);
            var user = await Repository.GetSingleModelAsync(a => a.id == infoEntity.userId);
            return infoEntity != null ? Mapper.Map<UserDTO>(user) : null;
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await Repository.GetSingleModelAsync(a => a.id == id);
            return Mapper.Map<UserDTO>(user);
        }

        public async Task<UserEntity> GetUserByConditionAsync(Expression<Func<UserEntity, bool>> whereLambda)
        {
            return await Repository.GetSingleModelAsync(whereLambda);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual async Task<bool> AddEntityAsync(UserSaveQuery t)
        {
            bool isOk;
            if (string.IsNullOrEmpty(t.accessToken) || string.IsNullOrEmpty(t.openId)) return false;
            var user = Mapper.Map<UserEntity>(t);
            var infoEntity = Mapper.Map<QQUserInfoEntity>(t);
            if (user != null)
            {
                //该用户已经登陆过了，只需要更新登录信息就行了
                user.lastLoginDate = DateTime.Now;
                user.loginCount += 1;
                user.nickName = t.nickName;
                user.portrait = t.portrait;
                infoEntity.userId = user.id;
                isOk = await UserRepository.UpdateAsync(user) &&
                       await UserQqInfoService.UpdateEntityAsync(infoEntity);
            }
            else
            {
                var defaultPwd = EncryptProvider.Base64Encrypt("123456"); //默认使用MD5加密密码         
                user = new UserEntity
                {
                    createDate = DateTime.Now,
                    portrait = t.portrait,
                    nickName = t.nickName,
                    loginTime = DateTime.Now,
                    lastLoginDate = DateTime.Now,
                    loginCount = 1,
                    disabled = false,
                    password = defaultPwd
                };
                infoEntity.accessToken = t.accessToken;
                infoEntity.openId = t.openId;
                infoEntity.nickName = t.nickName;
                infoEntity.figureurl_qq_1 = t.portrait;
                isOk = await UserRepository.AddAsync(user) && await UserQqInfoService.UpdateEntityAsync(infoEntity);
            }

            return isOk;
        }

        public async Task<List<UserDTO>> GetUsersNearVisitAsync(int count)
        {
            var users = await Repository.GetModelsAsync(a => a.id != 0);
            return Mapper.Map<List<UserDTO>>(users.OrderByDescending(a => a.createDate).Take(count).ToList());
        }

        public virtual async Task<bool> RemoveEntityAsync(int tId)
        {
            return await UserRepository.DeleteAsync(new UserEntity {id = tId});
        }

        /// <summary>
        /// 验证密码和用户名
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<UserEntity> ValidatePasswordAsync(string userName, string password)
        {
            password = EncryptProvider.Base64Encrypt(password);
            password = EncryptProvider.Md5(password);
            var userEntity =
                await UserRepository.GetSingleModelAsync(a => a.loginName == userName && a.password == password);
            return userEntity;
        }
    }
}