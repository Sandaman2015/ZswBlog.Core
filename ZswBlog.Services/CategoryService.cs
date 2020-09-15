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
        private readonly ICategoryRepoistory _categoryRepoistory;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepoistory categoryRepoistory, IMapper mapper)
        {
            _categoryRepoistory = categoryRepoistory;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取所有分类以及文章数量
        /// </summary>
        /// <returns></returns>
        public List<CategoryDTO> GetALLCategories() {
            List<CategoryEntity> categories = _categoryRepoistory.GetModels((CategoryEntity c)=>c.id !=0).ToList();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }
        
    }
}
