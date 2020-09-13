
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface ICommentService : IBaseService<CommentEntity>
    {
        /// <summary>
        /// 根据评论id获取该评论
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        Task<CommentEntity> GetCommentByIdAsync(int commentId);
        /// <summary>
        /// 获取所有评论
        /// </summary>
        /// <returns></returns>
        Task<List<CommentEntity>> GetAllCommentsAsync();
        /// <summary>
        /// 根据父评论Id获取父评论下的所有评论
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        Task<List<CommentEntity>> GetCommentsByTargetIdAsync(int targetId);
        /// <summary>
        /// 分页获取所有一级评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        Task<List<CommentEntity>> GetCommentsOnNotReplyAsyncByPageAsync(int articleId, int limit, int pageIndex);
        /// <summary>
        /// 获取所有一级评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<List<CommentEntity>> GetCommentsOnNotReplyAsync(int articleId);
        /// <summary>
        /// 获取一名用户最近的评论
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsExistsCommentOnNewestByUserId(int userId);

    }
}
