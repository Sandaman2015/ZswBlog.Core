using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.Query;

namespace ZswBlog.Common.Profiles
{
    public class TravelProfile : Profile
    {
        public TravelProfile()
        {
            CreateMap<TravelEntity, TravelDTO>().ForMember(e=>e.imgList, opts=>opts.MapFrom(a=>a.imgList.Select(d=>d.fileAttachment)));
            CreateMap<TravelSaveQuery, TravelEntity>();
        }
    }
}
