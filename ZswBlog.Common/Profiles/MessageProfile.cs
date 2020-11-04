using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.Common.Profiles
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public MessageProfile()
        {
            CreateMap<MessageEntity, MessageDTO>();
            CreateMap<MessageEntity, MessageTreeDTO>();
            CreateMap<MessageDTO, MessageTreeDTO>();
        }
    }
}
