using Microsoft.EntityFrameworkCore;

namespace ZswBlog.Entity
{
    public partial class SingleBlogContext : DbContext
    {
        public SingleBlogContext()
        {
        }

        public SingleBlogContext(DbContextOptions<SingleBlogContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<ArticleTag> ArticleTag { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<FriendLink> Friendlink { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<SiteTag> SiteTag { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<Travel> Travel { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Timeline> Timeline { get; set; }
        public virtual DbSet<FileAttachment> FileAttachments { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //if (!optionsBuilder.IsConfigured)
        //    //{
        //    //    //optionsBuilder.UseMySql("server=47.97.43.95;port=3306;database=singleblog;SslMode=None;uid=root;pwd=1234;Allow User Variables=true", x=>x.ServerVersio("8.0.19-mysql"));
        //    //    optionsBuilder.UseMySql("server=47.97.43.95;port=3306;database=singleblog;SslMode=None;uid=root;pwd=732668;Allow User Variables=true;");
        //    //}
        //    //else {
        //    //    base.OnConfiguring(optionsBuilder);
        //    //}            
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}