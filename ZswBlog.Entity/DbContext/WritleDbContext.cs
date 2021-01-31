using Microsoft.EntityFrameworkCore;

namespace ZswBlog.Entity.DbContext
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