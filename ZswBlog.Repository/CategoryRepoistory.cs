using System;
using System.Collections.Generic;
using System.Text;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class CategoryRepoistory : BaseRepository<CategoryEntity>, ICategoryRepoistory, IBaseRepository<CategoryEntity>
    {
    }
}
