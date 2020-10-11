using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.Core.Profiles
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ArticleProfile”的 XML 注释
    public class ArticleProfile : Profile
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ArticleProfile”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ArticleProfile.ArticleProfile()”的 XML 注释
        public ArticleProfile()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ArticleProfile.ArticleProfile()”的 XML 注释
        {
            CreateMap<ArticleEntity, ArticleDTO>()
              .ForMember(dest => dest.readTime, pro => pro.MapFrom(src => src.textCount / 250));
        }
    }
}
