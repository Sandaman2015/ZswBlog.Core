using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ZswBlog.DTO;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ZswBlogDbContext : DbContext
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
        public virtual DbSet<ArticleEntity> Article { get; set; }

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
        public virtual DbSet<FriendLinkEntity> FriendlinkEntities { get; set; }

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
        // 充血关联模型实体
        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<MessageDTO> MessageDTO { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<ArticleDTO> ArticleDTO { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<CommentDTO> CommentDTO { get; set; }
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
