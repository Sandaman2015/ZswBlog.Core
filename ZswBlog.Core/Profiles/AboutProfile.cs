using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace SingleBlog.Web.Profiles
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AboutProfile”的 XML 注释
    public class AboutProfile : Profile
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AboutProfile”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“AboutProfile.AboutProfile()”的 XML 注释
        public AboutProfile()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“AboutProfile.AboutProfile()”的 XML 注释
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
