using AutoMapper;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace SingleBlog.Web.Profiles
{
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��CommentProfile���� XML ע��
    public class CommentProfile : Profile
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��CommentProfile���� XML ע��
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��CommentProfile.CommentProfile()���� XML ע��
        public CommentProfile()
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��CommentProfile.CommentProfile()���� XML ע��
        {
            CreateMap<User, CommentDTO>();
            CreateMap<Comment, CommentDTO>()
              .ForMember(dest => dest.Comment, pro => pro.MapFrom(src => src.Comment1));
        }
    }
}
