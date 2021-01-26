using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;
using ZswBlog.IServices;
using ZswBlog.ThirdParty.Location;

namespace ZswBlog.Services
{
    public class MessageService : BaseService<MessageEntity, IMessageRepository>, IMessageService
    {
        public IMessageRepository MessageRepository { get; set; }
        public IMapper Mapper { get; set; }
        public IUserService UserService { get; set; }

        /// <summary>
        /// 获取留言详情
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task<MessageDTO> GetMessageByIdAsync(int messageId)
        {
            return await Task.Run(() =>
            {
                var message = MessageRepository.GetSingleModelAsync(a => a.id == messageId);
                return Mapper.Map<MessageDTO>(message);
            });
        }

        /// <summary>
        /// 用户上次留言时间是否大于1分钟
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsMessageOnNewestByUserId(int userId)
        {
            return await Task.Run(() =>
            {
                var messages = MessageRepository.GetModelsAsync(a => a.userId == userId).Result
                    .OrderByDescending(a => a.createDate).ToList();
                if (messages.Count <= 0) return true;
                var timeSpan = DateTime.Now - messages[0].createDate;
                return timeSpan.TotalMinutes > 1;
            });
        }

        /// <summary>
        /// 添加留言
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<bool> AddMessageAsync(MessageEntity t)
        {
            return await Task.Run(() =>
            {
                bool flag;
                var user = UserService.GetUserByIdAsync(t.userId);
                if (IsExistsMessageOnNewestByUserId(t.userId).Result)
                {
                    // 判断用户是否为空
                    if (user == null) return false;
                    t.location = LocationHelper.GetLocation(t.location);
                    flag = MessageRepository.AddAsync(t).Result;
                }
                else
                {
                    throw new Exception("你已经在一分钟前提交过一次了");
                }

                return flag;
            });
        }

        /// <summary>
        /// 删除留言
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveEntityAsync(int tId)
        {
            return await Task.Run(() =>
            {
                var message = MessageRepository.GetSingleModelAsync(a => a.id == tId).Result;
                return MessageRepository.DeleteAsync(message);
            });
        }

        /// <summary>
        /// 递归调用留言列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<PageDTO<MessageTreeDTO>> GetMessagesByRecursionAsync(int limit, int pageIndex)
        {
            return await Task.Run(() =>
            {
                var messageTopEntities = MessageRepository.GetModelsByPage(limit, pageIndex, false, a => a.createDate,
                    a => a.targetId == 0 && a.targetUserId == null, out var total).ToList();
                var messageTreeList = new List<MessageTreeDTO>();
                foreach (var item in messageTopEntities)
                {
                    var messageTree = Mapper.Map<MessageTreeDTO>(item);
                    ConvertMessageTree(messageTree);
                    var entities = MessageRepository.GetMessagesRecursiveAsync(item.id);
                    messageTree.children = Mapper.Map<List<MessageTreeDTO>>(entities);
                    messageTreeList.Add(messageTree);
                }

                return new PageDTO<MessageTreeDTO>(pageIndex, limit, total, messageTreeList);
            });
        }

        /// <summary>
        /// 用户信息填充
        /// </summary>
        /// <param name="treeDto"></param>
        private async Task ConvertMessageTree(MessageTreeDTO treeDto)
        {
            if (treeDto.targetId != 0)
            {
                var targetUser = RedisHelper.Get<UserDTO>("ZswBlog:UserInfo:" + treeDto.targetUserId);
                if (targetUser == null)
                {
                    targetUser = await UserService.GetUserByIdAsync(treeDto.targetUserId);
                    await RedisHelper.SetAsync("ZswBlog:UserInfo:" + treeDto.targetUserId, targetUser, 60 * 60 * 6);
                }

                treeDto.targetUserPortrait = targetUser.portrait;
                treeDto.targetUserName = targetUser.nickName;
            }

            var user = RedisHelper.Get<UserDTO>("ZswBlog:UserInfo:" + treeDto.userId);
            if (user != null) return;
            user = await UserService.GetUserByIdAsync(treeDto.userId);
            await RedisHelper.SetAsync("ZswBlog:UserInfo:" + treeDto.userId, user, 60 * 60 * 6);
        }

        public async Task<List<MessageDTO>> GetMessageOnNearSaveAsync(int count)
        {
            return await Task.Run(() => MessageRepository.GetMessageOnNearSaveAsync(count));
        }
    }
}