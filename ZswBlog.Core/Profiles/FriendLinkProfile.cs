using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace SingleBlog.Web.Profiles
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“FriendLinkProfile”的 XML 注释
    public class FriendLinkProfile : Profile
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“FriendLinkProfile”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“FriendLinkProfile.FriendLinkProfile()”的 XML 注释
        public FriendLinkProfile()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“FriendLinkProfile.FriendLinkProfile()”的 XML 注释
        {
            CreateMap<FriendLink, FriendLinkDTO>()
              .ForMember(dest => dest.LinkImage, pro => pro.MapFrom(src => src.FriendlinkImage))
              .ForMember(dest => dest.LinkIntroduce, pro => pro.MapFrom(src => src.FriendlinkIntroduce))
              .ForMember(dest => dest.LinkSrc, pro => pro.MapFrom(src => src.Friendlink))
              .ForMember(dest => dest.LinkTitle, pro => pro.MapFrom(src => src.FriendlinkTitle));
        }
    }
}
