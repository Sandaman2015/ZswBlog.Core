﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 文章评论实体对象
    /// </summary>
   public class CommentEntity
    {

        /// <summary>
        /// 评论id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createDate { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 所属文章id
        /// </summary>
        public int articleId { get; set; }
        /// <summary>
        /// 目标用户
        /// </summary>
        public int targetUserId { get; set; }
        /// <summary>
        /// 目标评论
        /// </summary>
        public int targetId { get; set; }
        /// <summary>
        /// 评论位置
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>
        public string browser { get; set; }
    }
}
