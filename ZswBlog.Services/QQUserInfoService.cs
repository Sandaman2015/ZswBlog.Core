using System;
using System.Collections.Generic;
using System.Text;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class QQUserInfoService : BaseService<QQUserInfoEntity, IQQUserInfoRepoistory>, IQQUserInfoService
    {
        private readonly IQQUserInfoRepoistory _userInfoRepoistory;        

        public QQUserInfoService(IQQUserInfoRepoistory userInfoRepoistory)
        {
            _userInfoRepoistory = userInfoRepoistory;
        }

        public QQUserInfoEntity GetQQUserInfoByOpenId(string openId)
        {
            return _userInfoRepoistory.GetSingleModel(a => a.openId == openId);
        }
    }
}
