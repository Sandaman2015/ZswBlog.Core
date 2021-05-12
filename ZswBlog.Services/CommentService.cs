using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;
using ZswBlog.ThirdParty.Location;

namespace ZswBlog.Services
{
    public class CommentService : BaseService<CommentEntity, ICommentRepository>, ICommentService
    {
        public ICommentRepository CommentRepository { get; set; }
        public IMapper Mapper { get; set; }
        public IUserService UserService { get; set; }

        /// <summary>
        /// 获取评论详情
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task<CommentDTO> GetCommentByIdAsync(int commentId)
        {
            var comment = await Repository.GetSingleModelAsync(a => a.id == commentId);
            return Mapper.Map<CommentDTO>(comment);
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommentDTO>> GetAllCommentsAsync()
        {
            return await Task.Run(() =>
            {
                var comments = Repository.GetModels(a => a.id != 0);
                return Mapper.Map<List<CommentDTO>>(comments.ToList());
            });
        }

        /// <summary>
        /// 根据目标id获取子集评论
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        public async Task<List<CommentDTO>> GetCommentsByTargetIdAsync(int targetId)
        {
            return await Task.Run(() =>
            {
                var comments = Repository.GetModels(a => a.targetId == targetId);
                return Mapper.Map<List<CommentDTO>>(comments.ToList());
            });
        }

        /// <summary>
        /// 分页获取一级留言
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<PageDTO<CommentDTO>> GetCommentsOnNotReplyByPageAsync(int articleId, int limit, int pageIndex)
        {
            return await Task.Run(() =>
            {
                var comments = Repository.GetModelsByPage(limit, pageIndex, false, (a => a.id),
                    (a => a.targetId == 0 && a.articleId == articleId), out var pageCount).ToList();
                var commentDtOs = Mapper.Map<List<CommentDTO>>(comments.ToList());
                return new PageDTO<CommentDTO>(pageIndex, limit, pageCount, commentDtOs);
            });
        }

        /// <summary>
        /// 根据id删除评论
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveEntityAsync(int tId)
        {
            return await Repository.DeleteAsync(new CommentEntity {id = tId});
        }

        /// <summary>
        /// 获取所有顶级评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<List<CommentDTO>> GetCommentsOnNotReplyAsync(int articleId)
        {
            return await Task.Run(() =>
            {
                var comments =  Repository.GetModels(a => a.id == 0 && a.articleId == articleId);
                return Mapper.Map<List<CommentDTO>>(comments.ToList());
            });
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<bool> AddCommentAsync(CommentEntity t)
        {
            bool flag;
            var user = await UserService.GetUserByIdAsync(t.userId);
            if (await IsExistsCommentOnNewestByUserIdAsync(t.userId))
            {
                // 判断用户是否为空
                if (user == null) return false;
                t.location = LocationHelper.GetLocation(t.location);
                flag = await CommentRepository.AddAsync(t);
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
        public async Task<bool> IsExistsCommentOnNewestByUserIdAsync(int userId)
        {
            return await Task.Run(() =>
            {
                var comments =  Repository.GetModels(a => a.userId == userId);
                if (comments.OrderByDescending(a => a.createDate).ToList().Count <= 0) return true;
                var timeSpan = DateTime.Now - comments.OrderByDescending(a => a.createDate).ToList()[0].createDate;
                var flag = timeSpan.TotalMinutes > 1;
                return flag;
            });
        }

        /// <summary>
        /// 递归调用
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<PageDTO<CommentTreeDTO>> GetCommentsByRecursionAsync(int limit, int pageIndex, int articleId)
        {
            var comments = await CommentRepository.GetModelsByPageAsync(limit, pageIndex, false, a => a.createDate,
                c => c.articleId == articleId && c.targetId == 0);
            var commentDtoList = new List<CommentTreeDTO>();
             foreach (var item in comments.data.ToList())
            {
                var commentTree = Mapper.Map<CommentTreeDTO>(item);
                await ConvertCommentTree(commentTree);
                var treeDtoList = await CommentRepository.GetCommentsRecursiveAsync(item.id, articleId);
                commentTree.children = Mapper.Map<List<CommentTreeDTO>>(treeDtoList.ToList());
                commentDtoList.Add(commentTree);
            }

            return new PageDTO<CommentTreeDTO>(pageIndex, limit, comments.count, commentDtoList);
        }

        /// <summary>
        /// 用户信息填充
        /// </summary>
        /// <param name="treeDto"></param>
        public async Task ConvertCommentTree(CommentTreeDTO treeDto)
        {
            if (treeDto.targetId != 0)
            {
                var targetUser = await RedisHelper.GetAsync<UserDTO>("ZswBlog:UserInfo:" + treeDto.targetUserId);
                if (targetUser == null)
                {
                    targetUser = await UserService.GetUserByIdAsync(treeDto.targetUserId);
                    await RedisHelper.SetAsync("ZswBlog:UserInfo:" + treeDto.targetUserId, targetUser, 60 * 60 * 6);
                }

                treeDto.targetUserPortrait = targetUser.portrait;
                treeDto.targetUserName = targetUser.nickName;
            }

            var user = await RedisHelper.GetAsync<UserDTO>("ZswBlog:UserInfo:" + treeDto.userId);
            if (user == null)
            {
                user = await UserService.GetUserByIdAsync(treeDto.userId);
                await RedisHelper.SetAsync("ZswBlog:UserInfo:" + treeDto.userId, user, 60 * 60 * 6);
            }

            treeDto.userPortrait = user.portrait;
            treeDto.userName = user.nickName;
        }
    }
}