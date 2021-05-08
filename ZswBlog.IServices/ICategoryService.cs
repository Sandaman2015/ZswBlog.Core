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
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
        /// <summary>
        /// 根据Id获取单个分类
        /// </summary>
        /// <returns></returns>
        Task<CategoryDTO> GetCategoryByIdAsync(int tId);
        /// <summary>
        /// 根据编码删除文章分类
        /// </summary>
        /// <param name="tid">文章分类id</param>
        /// <returns></returns>
        Task<bool> RemoveCatergoryByIdAsync(int tid);
    }
}
