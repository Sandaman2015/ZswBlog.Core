using AutoMapper;
using System.Collections.Generic;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.Admin.Profiles
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TagProfile”的 XML 注释
    public class TagProfile : Profile
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TagProfile”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TagProfile.TagProfile()”的 XML 注释
        public TagProfile()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TagProfile.TagProfile()”的 XML 注释
        {
            CreateMap<Article, MiniArticleDTO>()
             .ForMember(dest => dest.ArticleTime, pro => pro.MapFrom(src => src.ArticleCreateTime))
             .ForMember(dest => dest.ArticleContent, pro => pro.MapFrom(src => src.ArticleContent))
             .ForMember(dest => dest.ArticleTitle, pro => pro.MapFrom(src => src.ArticleTitle))
             .ForMember(dest => dest.ArticleId, pro => pro.MapFrom(src => src.ArticleId));

            CreateMap<Tag, ArticleTagDTO>()
              .ForMember(dest => dest.TagName, pro => pro.MapFrom(src => src.TagName));

            CreateMap<List<MiniArticleDTO>, ArticleTagDTO>()
            .ForMember(dest => dest.ArticleList, pro => pro.MapFrom(a => a));
        }

    }
}
