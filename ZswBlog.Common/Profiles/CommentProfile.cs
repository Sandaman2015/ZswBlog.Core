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
    public class CommentProfile:Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public CommentProfile()
        {
            CreateMap<CommentEntity,CommentDTO>();
            CreateMap<CommentEntity,CommentTreeDTO>();
            CreateMap<CommentDTO, CommentTreeDTO>();
        }
    }
}
