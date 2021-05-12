using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class CategoryService : BaseService<CategoryEntity, ICategoryRepository>, ICategoryService
    {
        public ICategoryRepository CategoryRepository { get; set; }
        public IArticleService ArticleService { get; set; }
        public IMapper Mapper { get; set; }

        /// <summary>
        /// 获取所有分类以及文章数量
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories =  CategoryRepository.GetModels(c => c.id != 0);
            var dToList = Mapper.Map<List<CategoryDTO>>(categories.ToList());
            foreach (var item in dToList)
            {
                item.articleCount = await ArticleService.GetArticleCountByCategoryIdAsync(item.id);
            }
            return dToList;
        }

        /// <summary>
        /// 根据分类Id获取详情
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public async Task<CategoryDTO> GetCategoryByIdAsync(int tId)
        {
            var category = await CategoryRepository.GetSingleModelAsync(c => c.id == tId);
            return Mapper.Map<CategoryDTO>(category);
        }
        /// <summary>
        /// 根据编码删除分类
        /// </summary>
        /// <param name="tid">分类编码</param>
        /// <returns></returns>
        public async Task<bool> RemoveCatergoryByIdAsync(int tid) {
            CategoryEntity entity = new CategoryEntity() { id = tid };
            return await CategoryRepository.DeleteAsync(entity);
        }
    }
}