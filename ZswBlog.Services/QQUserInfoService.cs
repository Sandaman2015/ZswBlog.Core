using AutoMapper;
using NETCore.Encrypt;
using System;
using System.Threading.Tasks;
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
            return await UserInfoRepository.GetSingleModelAsync(a => a.openId == openId);
        }

        /// <summary>
        /// 根据AccessToken获取新用户
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public virtual async Task<UserDTO> GetUserByAccessTokenAsync(string accessToken)
        {
            var login = new QQLogin();
            var openId = login.GetOpenID(accessToken);
            var qqUserInfo = login.GetQQUserInfo(accessToken, openId);
            if (qqUserInfo.Ret != 0 || !string.IsNullOrWhiteSpace(qqUserInfo.Msg)) return null;
            UserEntity user;
            var alreadyLoginUser = await GetQQUserInfoByOpenIdAsync(openId);
            //判断是否存在重复登陆且已经注册的用户
            if (alreadyLoginUser == null)
            {
                var defaultPwd = EncryptProvider.Md5("123456");//默认使用MD5加密密码         
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
                if (!await UserService.AddEntityAsync(user)) return null;
                var entity = new QQUserInfoEntity()
                {
                    openId = openId,
                    accessToken = accessToken,
                    userId = user.id,
                    gender = qqUserInfo.Gender,
                    figureurl_qq_1 = qqUserInfo.Figureurl_qq_1,
                    nickName = qqUserInfo.Nickname
                };
                if (await AddEntityAsync(entity))
                {
                    return Mapper.Map<UserDTO>(user);
                }
            }
            else
            {
                user = await UserService.GetUserByConditionAsync(a => a.id == alreadyLoginUser.userId && a.disabled == false);
                if (user == null)
                {
                    throw new Exception("该用户被禁止登陆！");
                }
                user.lastLoginDate = DateTime.Now;
                user.loginCount += 1;
                await UserService.UpdateEntityAsync(user);
                return Mapper.Map<UserDTO>(user);
            }
            return null;
        }
    }
}
