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
    public class SiteTagProfile:Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public SiteTagProfile()
        {
            CreateMap<SiteTagEntity, SiteTagDTO>().ForMember(a=>a.name,a=>a.MapFrom(b=>b.title));
        }
    }
}
