using Microsoft.EntityFrameworkCore;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SingleBlogContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public SingleBlogContext()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public SingleBlogContext(DbContextOptions<SingleBlogContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<Article> Article { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<ArticleTag> ArticleTag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<Comment> Comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<FriendLink> Friendlink { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<Message> Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<SiteTag> SiteTag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<Travel> Travel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<User> User { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<Timeline> Timeline { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<FileAttachment> FileAttachments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}