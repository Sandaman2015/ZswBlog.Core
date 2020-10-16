using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.DTO;
using ZswBlog.IServices;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 文章分类控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// 根据类型Id获取类型详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        [Route(template:"/category/get/{id}")]
        [HttpGet]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            return await Task.Run(() =>
            {
                return _categoryService.GetCategoryById(id);
            });
        }

        /// <summary>
        /// 获取所有文章类型
        /// </summary>
        /// <returns></returns>
        [Route("/category/get/all")]
        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetAllCategory()
        {
            return await Task.Run(() =>
            {
                return _categoryService.GetAllCategories();
            });
        }
    }
}
