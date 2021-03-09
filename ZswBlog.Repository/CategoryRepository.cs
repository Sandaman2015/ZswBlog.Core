using System;
using System.Collections.Generic;
using System.Text;
using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class CategoryRepository : BaseRepository<CategoryEntity>, ICategoryRepository, IBaseRepository<CategoryEntity>
    {
    }
}
