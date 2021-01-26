using System;
using System.Collections.Generic;
using System.Text;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class TravelFileAttachmentRepoistory : BaseRepository<TravelFileAttachmentEntity>, ITravelFileAttachmentRepoistory, IBaseRepository<TravelFileAttachmentEntity>
    {
    }
}
