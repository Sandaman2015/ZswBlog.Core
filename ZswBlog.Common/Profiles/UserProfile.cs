using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.Query;

namespace ZswBlog.Core.Profiles
{
    /// <summary>
    /// 
    /// </summary>
    public class UserProfile:Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public UserProfile()
        {
            CreateMap<UserEntity,UserDTO>();
            CreateMap<UserSaveQuery, UserEntity>();
            CreateMap<UserDTO, UserEntity>();
        }
    }
}
