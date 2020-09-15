using AutoMapper;
using System;
using System.Collections.Generic;
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
        private readonly IArticleService _articleService;

        public CategoryService(ICategoryRepoistory categoryRepoistory, IMapper mapper, IArticleService articleService)
        {
            _categoryRepoistory = categoryRepoistory;
            _mapper = mapper;
            _articleService = articleService;
        }

        
    }
}
