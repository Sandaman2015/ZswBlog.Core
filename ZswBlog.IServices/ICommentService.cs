
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
        CommentDTO GetCommentById(int commentId);
        /// <summary>
        /// 获取所有评论
        /// </summary>
        /// <returns></returns>
        List<CommentDTO> GetAllComments();
        /// <summary>
        /// 根据父评论Id获取父评论下的所有评论
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        List<CommentDTO> GetCommentsByTargetId(int targetId);
        /// <summary>
        /// 分页获取所有一级评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        PageDTO<CommentDTO> GetCommentsOnNotReplyByPage(int articleId, int limit, int pageIndex);
        /// <summary>
        /// 获取所有一级评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        List<CommentDTO> GetCommentsOnNotReply(int articleId);
        /// <summary>
        /// 获取一名用户最近的评论
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool IsExistsCommentOnNewestByUserId(int userId);

        /// <summary>
        /// 分页递归获取评论列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        PageDTO<CommentTreeDTO> GetCommentsByRecursion(int limit, int pageIndex);

    }
}
