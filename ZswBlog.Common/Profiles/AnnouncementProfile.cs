using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;

namespace ZswBlog.Common.Profiles
{
    /// <summary>
    /// 
    /// </summary>
    public class AnnouncementProfile:Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public AnnouncementProfile()
        {
            CreateMap<AnnouncementEntity, AnnouncementDTO>();
        }
    }
}
