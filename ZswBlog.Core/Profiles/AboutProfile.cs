using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace SingleBlog.Web.Profiles
{
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��AboutProfile���� XML ע��
    public class AboutProfile : Profile
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��AboutProfile���� XML ע��
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��AboutProfile.AboutProfile()���� XML ע��
        public AboutProfile()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��AboutProfile.AboutProfile()���� XML ע��
        {
            CreateMap<SiteTag, SiteTagDTO>()
              .ForMember(dest => dest.id, pro => pro.MapFrom(src => src.SitetagId))
              .ForMember(dest => dest.name, pro => pro.MapFrom(src => src.SitetagTitle));
            CreateMap<Timeline, TimeLineDTO>()
              .ForMember(dest => dest.id, pro => pro.MapFrom(src => src.TimelineId))
              .ForMember(dest => dest.time, pro => pro.MapFrom(src => src.TimelineCreateTime))
              .ForMember(dest => dest.title, pro => pro.MapFrom(src => src.TimelineTitle))
              .ForMember(dest => dest.content, pro => pro.MapFrom(src => src.TimtlineContent));
        }
    }
}
