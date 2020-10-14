using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.Core.Profiles
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
            CreateMap<SiteTagEntity, SiteTagDTO>();
        }
    }
}
