using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface ICategoryService:IBaseService<CategoryEntity>
    {
        /// <summary>
        /// 获取所有文章分类
        /// </summary>
        /// <returns></returns>
        List<CategoryDTO> GetALLCategories();
        /// <summary>
        /// 根据Id获取单个分类
        /// </summary>
        /// <returns></returns>
        CategoryDTO GetCategoryById(int tId);       
    }
}
