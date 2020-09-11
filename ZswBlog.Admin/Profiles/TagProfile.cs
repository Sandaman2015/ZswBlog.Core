using AutoMapper;
using System.Collections.Generic;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.Admin.Profiles
{
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��TagProfile���� XML ע��
    public class TagProfile : Profile
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��TagProfile���� XML ע��
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��TagProfile.TagProfile()���� XML ע��
        public TagProfile()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��TagProfile.TagProfile()���� XML ע��
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
