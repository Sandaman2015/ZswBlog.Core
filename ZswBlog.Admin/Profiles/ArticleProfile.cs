using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.Admin.Profiles
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ArticleProfile”的 XML 注释
    public class ArticleProfile : Profile
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ArticleProfile”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ArticleProfile.ArticleProfile()”的 XML 注释
        public ArticleProfile()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ArticleProfile.ArticleProfile()”的 XML 注释
        {
            CreateMap<Article, ArticleDTO>()
              .ForMember(dest => dest.ArticleReadTime, pro => pro.MapFrom(src => src.ArticleContent.Length / 250))
              .ForMember(dept => dept.ArticleCreatedBy, pro => pro.UseDestinationValue())
              .ForMember(dept => dept.ArticleTextCount, pro => pro.MapFrom(src => src.ArticleContent.Length))
              .ForMember(dept => dept.ArticleTime, pro => pro.MapFrom(src => src.ArticleCreateTime));
        }
    }
}
