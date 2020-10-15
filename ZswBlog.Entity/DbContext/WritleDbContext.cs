using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 写入数据库
    /// </summary>
    public class WritleDbContext : ZswBlogDbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public WritleDbContext()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public WritleDbContext(DbContextOptions<WritleDbContext> options)
            : base(options)
        {

        }

    }
}