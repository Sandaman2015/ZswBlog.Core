using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.Common.Profiles
{
    public class FileAttachmentProfile : Profile
    {
        public FileAttachmentProfile()
        {
            CreateMap<FileAttachmentEntity, FileAttachmentDTO>()
                .ForMember(a=>a.name, opts=>opts.MapFrom(d=>d.fileName))
                .ForMember(a=>a.url, opts => opts.MapFrom(d=>d.path));
        }
    }
}
