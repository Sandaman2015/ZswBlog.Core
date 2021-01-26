using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.Entity.DbContext
{
    /// <summary>
    /// 读取数据库
    /// </summary>
    public class ReadDbContext: ZswBlogDbContext
    { 
        /// <summary>
        /// 
        /// </summary>
        public ReadDbContext()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ReadDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
