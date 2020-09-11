using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.Admin.Profiles
{
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ArticleProfile���� XML ע��
    public class ArticleProfile : Profile
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ArticleProfile���� XML ע��
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ArticleProfile.ArticleProfile()���� XML ע��
        public ArticleProfile()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ArticleProfile.ArticleProfile()���� XML ע��
        {
            CreateMap<Article, ArticleDTO>()
              .ForMember(dest => dest.ArticleReadTime, pro => pro.MapFrom(src => src.ArticleContent.Length / 250))
              .ForMember(dept => dept.ArticleCreatedBy, pro => pro.UseDestinationValue())
              .ForMember(dept => dept.ArticleTextCount, pro => pro.MapFrom(src => src.ArticleContent.Length))
              .ForMember(dept => dept.ArticleTime, pro => pro.MapFrom(src => src.ArticleCreateTime));
        }
    }
}
