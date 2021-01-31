using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZswBlog.DTO;

namespace ZswBlog.Entity.DbContext
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ZswBlogDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public ZswBlogDbContext()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ZswBlogDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<ActionLogEntity> ActionLogEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<ArticleEntity> ArticleEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<ArticleTagEntity> ArticleTagEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<CommentEntity> CommentEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<FriendLinkEntity> FriendLinkEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<MessageEntity> MessageEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<SiteTagEntity> SiteTagEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<TagEntity> TagEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<TravelEntity> TravelEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<UserEntity> UserEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<TimeLineEntity> TimeLineEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<FileAttachmentEntity> FileAttachmentEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<QQUserInfoEntity> QQUserInfoEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<TravelFileAttachmentEntity> TravelFileAttachmentEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<CategoryEntity> CategoryEntities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<AnnouncementEntity> AnnouncementEntities { get; set; }
        /// <summary>
        /// 非模型实体
        /// </summary>
        public virtual DbSet<MessageDTO> MessageDTO { get; set; }
        /// <summary>
        /// 非模型实体
        /// </summary>
        public virtual DbSet<ArticleDTO> ArticleDTO { get; set; }
        /// <summary>
        /// 非模型实体
        /// </summary>
        public virtual DbSet<CommentDTO> CommentDTO { get; set; }
        /// <summary>
        /// Fluent API定义实体属性与关联
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleTagEntity>()
                .HasKey(t => new { t.articleId, t.tagId });

            modelBuilder.Entity<ArticleTagEntity>()
                .HasOne(pt => pt.article)
                .WithMany(p => p.articleTags)
                .HasForeignKey(pt => pt.articleId);

            modelBuilder.Entity<ArticleTagEntity>()
                .HasOne(pt => pt.tag)
                .WithMany(t => t.articleTags)
                .HasForeignKey(pt => pt.tagId);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
