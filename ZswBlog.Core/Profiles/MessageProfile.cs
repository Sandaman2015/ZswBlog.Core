using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace SingleBlog.Web.Profiles
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MessageProfile”的 XML 注释
    public class MessageProfile : Profile
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MessageProfile”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“MessageProfile.MessageProfile()”的 XML 注释
        public MessageProfile()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“MessageProfile.MessageProfile()”的 XML 注释
        {
            CreateMap<User, MessageDTO>();
            CreateMap<Message, MessageDTO>()
              .ForMember(dest => dest.Message, pro => pro.MapFrom(src => src.Message1));
        }
    }
}
