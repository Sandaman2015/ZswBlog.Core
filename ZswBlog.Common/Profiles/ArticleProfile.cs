using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.Query;

namespace ZswBlog.Common.Profiles
{
    /// <summary>
    /// 
    /// </summary>
    public class ArticleProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public ArticleProfile()
        {
            CreateMap<ArticleEntity, ArticleDTO>();
            CreateMap<ArticleSaveQuery, ArticleEntity>();
        }
    }
}
