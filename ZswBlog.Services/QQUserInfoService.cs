using AutoMapper;
using NETCore.Encrypt;
using System;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;
using ZswBlog.ThirdParty;

namespace ZswBlog.Services
{
    public class QQUserInfoService : BaseService<QQUserInfoEntity, IQQUserInfoRepoistory>, IQQUserInfoService
    {
        public IQQUserInfoRepoistory _userInfoRepoistory { get; set; }

        public IUserService _userService { get; set; }

        public IMapper _mapper { get; set; } 


        public QQUserInfoEntity GetQQUserInfoByOpenId(string openId)
        {
            return _userInfoRepoistory.GetSingleModel(a => a.openId == openId);
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
                QQUserInfoEntity alreadLoginUser = GetQQUserInfoByOpenId(openId);
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
                        password = defaultPwd
                    };
                    if (_userService.AddEntity(user))
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
                        if (AddEntity(entity))
                        {
                            return _mapper.Map<UserDTO>(user);
                        }
                    }
                }
                else
                {
                    user = _userService.GetUserByCondition(a => a.id == alreadLoginUser.userId && a.disabled == false);
                    if (user == null)
                    {
                        throw new Exception("该用户被禁止登陆！");
                    }
                    user.lastLoginDate = DateTime.Now;
                    user.loginCount += 1;
                    _userService.UpdateEntity(user);
                    return _mapper.Map<UserDTO>(user);
                }
            }
            return null;
        }
    }
}
