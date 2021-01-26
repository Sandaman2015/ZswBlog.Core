using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.Core.config;
using ZswBlog.DTO;
using ZswBlog.IServices;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 文章分类控制器
    /// </summary>
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="categoryService"></param>
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// 根据类型Id获取类型详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        [Route(template: "/api/category/get/{id}")]
        [HttpGet]
        [FunctionDescription("根据类型Id获取类型详情")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            return await Task.Run(() => _categoryService.GetCategoryByIdAsync(id));
        }

        /// <summary>
        /// 获取所有文章类型
        /// </summary>
        /// <returns></returns>
        [Route("/api/category/get/all")]
        [HttpGet]
        [FunctionDescription("获取所有文章类型")]
        public async Task<ActionResult<List<CategoryDTO>>> GetAllCategory()
        {
            return await Task.Run(() => _categoryService.GetAllCategoriesAsync());
        }
    }
}
