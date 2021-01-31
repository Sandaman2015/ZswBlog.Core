using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.Query;
using System.Linq;

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
            CreateMap<ArticleEntity, ArticleDTO>()
                .ForMember(dest => dest.tags,
                opts => opts
                .MapFrom(a => a.articleTags.Select(a => a.tag)));
            CreateMap<ArticleDTO, ArticleEntity>();
            CreateMap<ArticleSaveQuery, ArticleEntity>();
            CreateMap<ArticleUpdateQuery, ArticleEntity>()
                .ForMember(dest => dest.createDate, opt => opt.Ignore())
                .ForMember(dest => dest.visits, opt=>opt.Ignore());
            
        }
    }
}
