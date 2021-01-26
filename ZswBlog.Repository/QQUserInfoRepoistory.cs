using System;
using System.Collections.Generic;
using System.Text;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class QQUserInfoRepoistory : BaseRepository<QQUserInfoEntity>, IQQUserInfoRepoistory, IBaseRepository<QQUserInfoEntity>
    {
    }
}
