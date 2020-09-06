using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace SingleBlog.Web.Profiles
{
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��FriendLinkProfile���� XML ע��
    public class FriendLinkProfile : Profile
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��FriendLinkProfile���� XML ע��
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��FriendLinkProfile.FriendLinkProfile()���� XML ע��
        public FriendLinkProfile()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��FriendLinkProfile.FriendLinkProfile()���� XML ע��
        {
            CreateMap<FriendLink, FriendLinkDTO>()
              .ForMember(dest => dest.LinkImage, pro => pro.MapFrom(src => src.FriendlinkImage))
              .ForMember(dest => dest.LinkIntroduce, pro => pro.MapFrom(src => src.FriendlinkIntroduce))
              .ForMember(dest => dest.LinkSrc, pro => pro.MapFrom(src => src.Friendlink))
              .ForMember(dest => dest.LinkTitle, pro => pro.MapFrom(src => src.FriendlinkTitle));
        }
    }
}
