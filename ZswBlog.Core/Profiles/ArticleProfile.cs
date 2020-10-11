using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.Core.Profiles
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
            CreateMap<ArticleEntity, ArticleDTO>()
              .ForMember(dest => dest.readTime, pro => pro.MapFrom(src => src.textCount / 250));
        }
    }
}
