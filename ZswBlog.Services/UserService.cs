using Autofac.Extras.DynamicProxy;
using AutoMapper;
using NETCore.Encrypt;
using System;
using System.Collections.Generic;
using System.Linq;
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
                        string defaultPwd = EncryptProvider.Md5("123456");//默认使用MD5加密密码         
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
        /// 根据AccessToken获取新用户
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public virtual UserDTO GetUserByAccessToken(string accessToken)
        {
            QQLogin login = new QQLogin();
            string openId = login.GetOpenID(accessToken);
            QQUserInfo qqUserInfo = login.GetQQUserInfo(accessToken, openId);
            if (qqUserInfo.Ret == 0 && string.IsNullOrWhiteSpace(qqUserInfo.Msg)) //成功
            {
                UserEntity user;
                QQUserInfoEntity alreadLoginUser = _userQQInfoService.GetQQUserInfoByOpenId(openId);
                //判断是否存在重复登陆且已经注册的用户
                if (alreadLoginUser == null)
                {
                    string defaultPwd = EncryptProvider.Md5("123456");//默认使用MD5加密密码         
                    user = new UserEntity()
                    {
                        createDate = DateTime.Now,
                        portrait = qqUserInfo.Figureurl_qq_1,
                        nickName = qqUserInfo.Nickname,
                        loginTime = DateTime.Now,
                        lastLoginDate = DateTime.Now,
                        loginCount = 1,
                        disabled = false,
                        password= defaultPwd
                    };
                    if (_userRepository.Add(user))
                    {
                        QQUserInfoEntity entity = new QQUserInfoEntity()
                        {
                            openId = openId,
                            accessToken = accessToken,
                            userId = user.id,
                            gender = qqUserInfo.Gender,
                            figureurl_qq_1 = qqUserInfo.Figureurl_qq_1,
                            nickName = qqUserInfo.Nickname
                        };
                        if (_userQQInfoService.AddEntity(entity))
                        {
                            return _mapper.Map<UserDTO>(user);
                        }
                    }
                }
                else
                {
                    user = _userRepository.GetSingleModel(a => a.id == alreadLoginUser.userId && a.disabled == false);
                    if (user == null) {
                        throw new Exception("该用户被禁止登陆！");
                    }
                    user.lastLoginDate = DateTime.Now;
                    user.loginCount = 1;
                    _userRepository.Update(user);
                    return _mapper.Map<UserDTO>(user);
                }
            }
            return null;
        }
    }
}
