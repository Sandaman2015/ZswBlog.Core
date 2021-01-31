using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface IQQUserInfoService:IBaseService<QQUserInfoEntity>
    {

        /// <summary>
        /// 根据开放id 获取用户
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        Task<QQUserInfoEntity> GetQQUserInfoByOpenIdAsync(string openId);

        /// <summary>
        /// 根据第三方Token获取用户信息
        /// </summary>
        /// <returns></returns>
        Task<UserDTO> GetUserByAccessTokenAsync(string accessToken);
    }
}
