using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class CategoryService : BaseService<CategoryEntity, ICategoryRepoistory>, ICategoryService
    {
        public ICategoryRepoistory _categoryRepoistory { get; set; }
        public IArticleService _articleService { get; set; }
        public IMapper _mapper { get; set; }

        /// <summary>
        /// 获取所有分类以及文章数量
        /// </summary>
        /// <returns></returns>
        public List<CategoryDTO> GetAllCategories()
        {
            List<CategoryEntity> categories = _categoryRepoistory.GetModels((CategoryEntity c) => c.id != 0).ToList();
            List<CategoryDTO> dTOList= _mapper.Map<List<CategoryDTO>>(categories);
            foreach (var item in dTOList) {
                item.articleCount = _articleService.GetArticleCountByCategoryId(item.id);
            }
            return dTOList;
        }
        /// <summary>
        /// 根据分类Id获取详情
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public CategoryDTO GetCategoryById(int tId)
        {
            CategoryEntity category = _categoryRepoistory.GetSingleModel((CategoryEntity c) => c.id == tId);
            return _mapper.Map<CategoryDTO>(category);
        }
    }
}
