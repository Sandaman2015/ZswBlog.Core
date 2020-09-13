using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface ICategoryService:IBaseService<CategoryEntity>
    {
        Task<List<ArticleEntity>> GetArticleListByCategoryId(int categoryId);
    }
}
