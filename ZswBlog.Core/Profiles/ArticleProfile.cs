using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.Core.Profiles
{
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ArticleProfile���� XML ע��
    public class ArticleProfile : Profile
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ArticleProfile���� XML ע��
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ArticleProfile.ArticleProfile()���� XML ע��
        public ArticleProfile()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��ArticleProfile.ArticleProfile()���� XML ע��
        {
            CreateMap<ArticleEntity, ArticleDTO>()
              .ForMember(dest => dest.readTime, pro => pro.MapFrom(src => src.textCount / 250));
        }
    }
}
