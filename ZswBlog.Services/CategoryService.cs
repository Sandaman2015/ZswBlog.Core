using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class CategoryService : BaseService<CategoryEntity, ICategoryRepoistory>, ICategoryService
    {
        public ICategoryRepoistory CategoryRepository { get; set; }
        public IArticleService ArticleService { get; set; }
        public IMapper Mapper { get; set; }

        /// <summary>
        /// 获取所有分类以及文章数量
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            return await Task.Run(() =>
            {
                var categories = CategoryRepository.GetModelsAsync((CategoryEntity c) => c.id != 0).Result.ToList();
                var dToList = Mapper.Map<List<CategoryDTO>>(categories);
                foreach (var item in dToList)
                {
                    item.articleCount = ArticleService.GetArticleCountByCategoryIdAsync(item.id).Result;
                }
                return dToList;
            });
        }

        /// <summary>
        /// 根据分类Id获取详情
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public async Task<CategoryDTO> GetCategoryByIdAsync(int tId)
        {
            var category = await CategoryRepository.GetSingleModelAsync((CategoryEntity c) => c.id == tId);
            return Mapper.Map<CategoryDTO>(category);
        }
    }
}