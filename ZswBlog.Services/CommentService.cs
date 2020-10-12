using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;
using ZswBlog.ThirdParty.Location;

namespace ZswBlog.Services
{
    public class CommentService : BaseService<CommentEntity, ICommentRepository>, ICommentService
    {
        public ICommentRepository _commentRepository { get; set; }
        public IMapper _mapper { get; set; }
        public IUserService _userService { get; set; }

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
            List<CommentEntity> comments = _repository.GetModelsByPage(limit, pageIndex, false, (a => a.id), (a => a.targetId == 0 && a.articleId == articleId), out int pageCount).ToList();
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
            return _repository.Delete(new CommentEntity { id = tId });
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
        /// 添加评论
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override bool AddEntity(CommentEntity t)
        {
            bool flag = false;
            UserDTO user = _userService.GetUserById(t.userId);
            if (IsExistsCommentOnNewestByUserId(t.userId))
            {
                // 判断用户是否为空
                if (user != null)
                {
                    t.location = LocationHelper.GetLocation(t.location);
                    flag = _commentRepository.Add(t);
                }
            }
            else
            {
                throw new Exception("你已经在一分钟前提交过一次了");
            }
            return flag;
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
        public PageDTO<CommentTreeDTO> GetCommentsByRecursion(int limit, int pageIndex, int articleId)
        {
            List<CommentEntity> comments = _commentRepository.GetModelsByPage(limit, pageIndex, false, a => a.createDate, a => a.targetId != 0 && a.articleId == articleId, out int pageCount).ToList();
            List<CommentTreeDTO> commentDTOs = _mapper.Map<List<CommentTreeDTO>>(comments);
            foreach (CommentTreeDTO commentTree in commentDTOs)
            {
                ConvertCommentTree(commentTree);
                commentTree.children = new List<CommentTreeDTO>();
                RecursionComments(commentTree, commentTree.id, new List<CommentTreeDTO>());
            }
            return new PageDTO<CommentTreeDTO>(pageIndex, limit, pageCount, commentDTOs);
        }

        /// <summary>
        /// 清空列表
        /// </summary>
        /// <returns></returns>
        private bool RecursionComments(CommentTreeDTO treeDTO, int targetId, List<CommentTreeDTO> commentTrees)
        {
            List<CommentEntity> comments = _commentRepository.GetModels((CommentEntity ce) => ce.targetId == targetId).ToList();
            if (comments.Count > 0)
            {
                List<CommentTreeDTO> commentTree = _mapper.Map<List<CommentTreeDTO>>(comments);
                foreach (CommentTreeDTO comment in commentTree)
                {
                    ConvertCommentTree(comment);
                    commentTrees.Add(comment);
                }
                return RecursionComments(treeDTO, commentTree[0].id, commentTrees);
            }
            else
            {
                commentTrees = commentTrees.OrderBy(e => e.createDate).ToList();
                treeDTO.children.AddRange(commentTrees);
                return true;
            }
        }
        /// <summary>
        /// 用户信息填充
        /// </summary>
        /// <param name="treeDTO"></param>
        public void ConvertCommentTree(CommentTreeDTO treeDTO)
        {
            if (treeDTO.targetId != 0)
            {
                UserDTO targetUser = RedisHelper.Get<UserDTO>("ZswBlog:UserInfo:" + treeDTO.targetUserId);
                if (targetUser == null)
                {
                    targetUser = _userService.GetUserById(treeDTO.targetUserId);
                    RedisHelper.Set("ZswBlog:UserInfo:" + treeDTO.targetUserId, targetUser, 60 * 60 * 6);
                }
                treeDTO.targetUserPortrait = targetUser.portrait;
                treeDTO.targetUserName = targetUser.nickName;
            }
            UserDTO user = RedisHelper.Get<UserDTO>("ZswBlog:UserInfo:" + treeDTO.userId);
            if (user == null)
            {
                user = _userService.GetUserById(treeDTO.userId);
                RedisHelper.Set("ZswBlog:UserInfo:" + treeDTO.userId, user, 60 * 60 * 6);
            }
            treeDTO.userPortrait = user.portrait;
            treeDTO.userName = user.nickName;
        }
    }
}
