
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
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
        Task<CommentDTO> GetCommentByIdAsync(int commentId);
        /// <summary>
        /// 获取所有评论
        /// </summary>
        /// <returns></returns>
        Task<List<CommentDTO>> GetAllCommentsAsync();
        /// <summary>
        /// 根据父评论Id获取父评论下的所有评论
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        Task<List<CommentDTO>> GetCommentsByTargetIdAsync(int targetId);
        /// <summary>
        /// 分页获取所有一级评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        Task<PageDTO<CommentDTO>> GetCommentsOnNotReplyByPageAsync(int articleId, int limit, int pageIndex);
        /// <summary>
        /// 获取所有一级评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<List<CommentDTO>> GetCommentsOnNotReplyAsync(int articleId);
        /// <summary>
        /// 获取一名用户最近的评论
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsExistsCommentOnNewestByUserIdAsync(int userId);
        /// <summary>
        /// 分页递归获取评论列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<PageDTO<CommentTreeDTO>> GetCommentsByRecursionAsync(int limit, int pageIndex, int articleId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<bool> AddCommentAsync(CommentEntity t);
    }
}
