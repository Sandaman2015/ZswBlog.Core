using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
        Task<UserEntity> GetUserByOpenIdAsync(string openId);
    }
}
