using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.Common.Profiles
{
    public class TagProfile:Profile
    {
        public TagProfile()
        {
            CreateMap<TagEntity, TagDTO>();
        }
    }
}
