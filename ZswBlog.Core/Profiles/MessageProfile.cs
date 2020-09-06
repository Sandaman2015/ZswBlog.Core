using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace SingleBlog.Web.Profiles
{
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��MessageProfile���� XML ע��
    public class MessageProfile : Profile
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��MessageProfile���� XML ע��
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��MessageProfile.MessageProfile()���� XML ע��
        public MessageProfile()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��MessageProfile.MessageProfile()���� XML ע��
        {
            CreateMap<User, MessageDTO>();
            CreateMap<Message, MessageDTO>()
              .ForMember(dest => dest.Message, pro => pro.MapFrom(src => src.Message1));
        }
    }
}
