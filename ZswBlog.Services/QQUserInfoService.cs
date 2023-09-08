using AutoMapper;
using NETCore.Encrypt;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.Common.Exception;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;
using ZswBlog.ThirdParty;

namespace ZswBlog.Services
{
    public class QQUserInfoService : BaseService<QQUserInfoEntity, IQQUserInfoRepository>, IQQUserInfoService
    {
        public IQQUserInfoRepository UserInfoRepository { get; set; }

        public IUserService UserService { get; set; }

        public IMapper Mapper { get; set; }


        public async Task<QQUserInfoEntity> GetQQUserInfoByOpenIdAsync(string openId)
        {
            return await Task.Run(() =>
            {
                return UserInfoRepository.GetSingleModel(a => a.openId == openId);
            });
            
        }

        /// <summary>
        /// 根据AccessToken获取新用户
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public virtual async Task<UserDTO> GetUserByAccessTokenAsync(string accessToken)
        {
            UserEntity user;
            QQLogin login =  new QQLogin();
            var openId = await login.GetOpenID(accessToken);
            var qqUserInfo = await login.GetQQUserInfo(accessToken, openId);
            if (qqUserInfo.Ret != 0 || !string.IsNullOrWhiteSpace(qqUserInfo.Msg)) {
                return new UserDTO();
            }
            var alreadyLoginUser = await GetQQUserInfoByOpenIdAsync(openId);
            //判断是否存在重复登陆且已经注册的用户
            if (alreadyLoginUser == null)
            {
                //默认使用MD5加密密码
                var defaultPwd = EncryptProvider.Md5("123456");
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
                if (!UserService.AddEntity(user)) return null;
                var entity = new QQUserInfoEntity()
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
                    return Mapper.Map<UserDTO>(user);
                }
            }
            else
            {
                user = await UserService.GetUserByConditionAsync(a => a.id == alreadyLoginUser.userId && a.disabled == false);
                if (user == null)
                {
                    throw new BusinessException("该用户被禁止登陆！", 401);
                }
                user.lastLoginDate = DateTime.Now;
                user.loginCount += 1;
                UserService.UpdateEntity(user);
                return Mapper.Map<UserDTO>(user);
            }
            return null;
        }
    }
}
