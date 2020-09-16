using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class CommentService : BaseService<CommentEntity, ICommentRepository>, ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        public CommentService(ICommentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        /// <summary>
        /// 获取评论详情
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public CommentDTO GetCommentById(int commentId)
        {
            CommentEntity comment = _repository.GetSingleModel(a => a.id == commentId);
            return _mapper.Map<CommentDTO>(comment);
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public List<CommentDTO> GetAllComments()
        {
            List<CommentEntity> comments = _repository.GetModels(a => a.id != 0).ToList();
            return _mapper.Map<List<CommentDTO>>(comments);
        }

        /// <summary>
        /// 根据目标id获取子集评论
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        public List<CommentDTO> GetCommentsByTargetId(int targetId)
        {
            List<CommentEntity> comments = _repository.GetModels(a => a.targetId == targetId).ToList();
            return _mapper.Map<List<CommentDTO>>(comments);
        }

        /// <summary>
        /// 分页获取一级留言
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public PageDTO<CommentDTO> GetCommentsOnNotReplyByPage(int articleId, int limit, int pageIndex)
        {
            int pageCount;
            List<CommentEntity> comments = _repository.GetModelsByPage(limit, pageIndex, false, (a => a.id), (a => a.targetId == 0 && a.articleId == articleId), out pageCount).ToList();
            List<CommentDTO> commentDTOs = _mapper.Map<List<CommentDTO>>(comments);
            return new PageDTO<CommentDTO>(pageIndex, limit, pageCount, commentDTOs);
        }
        /// <summary>
        /// 根据id删除评论
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public bool RemoveEntity(int tId)
        {
            CommentEntity comment = _repository.GetSingleModel(a => a.id == tId);
            return _repository.Delete(comment);
        }

        /// <summary>
        /// 获取所有顶级评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public List<CommentDTO> GetCommentsOnNotReply(int articleId)
        {
            List<CommentEntity> comments = _repository.GetModels(a => a.id == 0 && a.articleId == articleId).ToList();
            return _mapper.Map<List<CommentDTO>>(comments);
        }

        /// <summary>
        /// 评论时间小于1分钟的用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsExistsCommentOnNewestByUserId(int userId)
        {
            bool flag = true;
            List<CommentEntity> comments = _repository.GetModels(a => a.userId == userId).OrderByDescending(a => a.createDate).ToList();
            if (comments != null && comments.Count > 0)
            {
                TimeSpan timeSpan = DateTime.Now - comments[0].createDate;
                flag = timeSpan.TotalMinutes < 1;
            }
            return flag;
        }

        /// <summary>
        /// 递归调用
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        public PageDTO<CommentTreeDTO> GetCommentsByRecursion(int limit, int pageIndex)
        {
            int pageCount;
            List<CommentEntity> comments = _commentRepository.GetModelsByPage(limit,pageIndex,false,a=>a.createDate,a => a.targetId != null,out pageCount).ToList();
            List<CommentTreeDTO> commentDTOs = _mapper.Map<List<CommentTreeDTO>>(comments);
            foreach (CommentTreeDTO commentTree in commentDTOs) {
                RecursionComments(commentTree,commentTree.id);
            }
            return new PageDTO<CommentTreeDTO>(pageIndex, limit, pageCount, commentDTOs);
        }

        /// <summary>
        /// 清空列表
        /// </summary>
        /// <returns></returns>
        private bool RecursionComments(CommentTreeDTO treeDTO , int targetId)
        {
            List<CommentEntity> comments = _commentRepository.GetModels((CommentEntity ce) => ce.targetId == targetId).ToList();
            if (comments.Count > 0) {
                List<CommentTreeDTO> commentTree = _mapper.Map<List<CommentTreeDTO>>(comments);
                foreach (CommentTreeDTO comment in commentTree) {                    
                    treeDTO.children.Add(comment);
                    return RecursionComments(treeDTO, comment.id);
                }
                return false;
            }
            else {
                return true;
            }
        }

    }
}
