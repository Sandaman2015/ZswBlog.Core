using Autofac.Extras.DynamicProxy;
using AutoMapper;
using NETCore.Encrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZswBlog.Common.AopConfig;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;
using ZswBlog.Query;
using ZswBlog.ThirdParty;

namespace ZswBlog.Services
{
    [Intercept(typeof(EnableTransaction))]
    public class UserService : BaseService<UserEntity, IUserRepository>, IUserService
    {
        public IUserRepository _userRepository { get; set; }
        public IMapper _mapper { get; set; }
        public IQQUserInfoService _userQQInfoService { get; set; }

        public List<UserDTO> GetAllUsers()
        {
            List<UserEntity> users = _userRepository.GetModels(a => a.id != 0).ToList();
            return _mapper.Map<List<UserDTO>>(users);
        }

        public UserDTO GetUserByOpenId(string openId)
        {
            QQUserInfoEntity infoEntity = _userQQInfoService.GetQQUserInfoByOpenId(openId);
            if (infoEntity != null)
            {
                UserEntity user = _repository.GetSingleModel(a => a.id == infoEntity.userId);
                return _mapper.Map<UserDTO>(user);
            }
            else
            {
                return null;
            }
        }

        public UserDTO GetUserById(int id)
        {
            UserEntity user = _repository.GetSingleModel(a => a.id == id);
            return _mapper.Map<UserDTO>(user);
        }

        public UserEntity GetUserByCondition(Expression<Func<UserEntity, bool>> whereLambda) {
            return _repository.GetSingleModel(whereLambda);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual bool AddEntity(UserSaveQuery t)
        {
            try
            {
                bool isOk = false;
                if (!string.IsNullOrEmpty(t.accessToken) && !string.IsNullOrEmpty(t.openId))
                {

                    UserEntity user = _mapper.Map<UserEntity>(t);
                    QQUserInfoEntity infoEntity = _mapper.Map<QQUserInfoEntity>(t);
                    if (user != null)
                    {
                        //该用户已经登陆过了，只需要更新登录信息就行了
                        user.lastLoginDate = DateTime.Now;
                        user.loginCount += 1;
                        user.nickName = t.nickName;
                        user.portrait = t.portrait;
                        infoEntity.userId = user.id;
                        isOk = _userRepository.Update(user) && _userQQInfoService.UpdateEntity(infoEntity);
                    }
                    else
                    {
                        string defaultPwd = EncryptProvider.Base64Encrypt("123456");//默认使用MD5加密密码         
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
                        isOk = _userRepository.Add(user) && _userQQInfoService.UpdateEntity(infoEntity);
                    }
                }
                return isOk;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserDTO> GetUsersNearVisit(int count)
        {
            List<UserEntity> users = _repository.GetModels(a => a.id != 0).OrderByDescending(a => a.createDate).Take(count).ToList();
            return _mapper.Map<List<UserDTO>>(users);
        }

        public virtual bool RemoveEntity(int tId)
        {
            return _userRepository.Delete(new UserEntity { id = tId });
        }

        /// <summary>
        /// 验证密码和用户名
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserEntity ValidatePassword(string userName, string password)
        {
            try
            {
                password = EncryptProvider.Base64Decrypt(password);
                UserEntity userEntity = _userRepository.GetSingleModel(a => a.nickName == userName && a.password == password);
                return userEntity;
            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}
