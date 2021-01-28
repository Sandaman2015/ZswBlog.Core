using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;
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
            CreateMap<ArticleDTO, ArticleEntity>();
            CreateMap<ArticleSaveQuery, ArticleEntity>();
            CreateMap<ArticleUpdateQuery, ArticleEntity>()
                .ForMember(dest => dest.createDate, opt => opt.Ignore())
                .ForMember(dest => dest.visits, opt=>opt.Ignore());
            
        }
    }
}
