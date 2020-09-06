using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace SingleBlog.Web.Profiles
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“CommentProfile”的 XML 注释
    public class CommentProfile : Profile
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“CommentProfile”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“CommentProfile.CommentProfile()”的 XML 注释
        public CommentProfile()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“CommentProfile.CommentProfile()”的 XML 注释
        {
            CreateMap<User, CommentDTO>();
            CreateMap<Comment, CommentDTO>()
              .ForMember(dest => dest.Comment, pro => pro.MapFrom(src => src.Comment1));
        }
    }
}
