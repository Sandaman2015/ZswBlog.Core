using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.Common.Profiles
{
    public class TravelProfile : Profile
    {
        public TravelProfile()
        {
            CreateMap<TravelEntity, TravelDTO>();
        }
    }
}
