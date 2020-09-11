using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.Admin.Profiles
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserProfile”的 XML 注释
    public class UserProfile : Profile
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserProfile”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“UserProfile.UserProfile()”的 XML 注释
        public UserProfile()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“UserProfile.UserProfile()”的 XML 注释
        {
            CreateMap<User, UserDTO>()
              .ForMember(dest => dest.UserId, pro => pro.MapFrom(src => src.UserId))
              .ForMember(dest => dest.UserName, pro => pro.MapFrom(src => src.UserName))
              .ForMember(dest => dest.UserPortrait, pro => pro.MapFrom(src => src.UserPortrait));
        }
    }
}
