using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.Admin.Profiles
{
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��UserProfile���� XML ע��
    public class UserProfile : Profile
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��UserProfile���� XML ע��
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��UserProfile.UserProfile()���� XML ע��
        public UserProfile()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��UserProfile.UserProfile()���� XML ע��
        {
            CreateMap<User, UserDTO>()
              .ForMember(dest => dest.UserId, pro => pro.MapFrom(src => src.UserId))
              .ForMember(dest => dest.UserName, pro => pro.MapFrom(src => src.UserName))
              .ForMember(dest => dest.UserPortrait, pro => pro.MapFrom(src => src.UserPortrait));
        }
    }
}
