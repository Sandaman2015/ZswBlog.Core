using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.Common;
using ZswBlog.Core.config;
using ZswBlog.DTO;
using ZswBlog.Entity;
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
            var data = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(data);
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
            var data =await Task.Run(() => _categoryService.GetAllCategoriesAsync());
            return Ok(data);
        }

        /// <summary>
        /// 保存文章类型
        /// </summary>
        /// <returns></returns>
        [Route("/api/category/admin/update")]
        [HttpPost]
        [Authorize]
        [FunctionDescription("更新文章类型")]
        public async Task<ActionResult<bool>> UpdateCategory([FromBody]CategoryEntity category) {
            return Ok(await _categoryService.UpdateEntityAsync(category));
        }

        /// <summary>
        /// 删除文章类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/api/category/admin/remove/{id}")]
        [HttpDelete]
        [Authorize]
        [FunctionDescription("删除文章类型")]
        public async Task<ActionResult<bool>> RemoveCategory([FromRoute]int id)
        {
            return Ok(await _categoryService.RemoveCatergoryByIdAsync(id));
        }

        /// <summary>
        /// 新增文章类型
        /// </summary>
        /// <param name="entity">保存实体</param>
        /// <returns></returns>
        [Route("/api/category/admin/save")]
        [HttpPost]
        [Authorize]
        [FunctionDescription("新增文章类型")]
        public async Task<ActionResult<bool>> SaveCategory([FromBody] CategoryEntity entity)
        {
            entity.createDate = DateTime.Now;
            return Ok(await _categoryService.AddEntityAsync(entity));
        }
    }
}
