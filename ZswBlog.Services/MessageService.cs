using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
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
                var messages =  MessageRepository.GetModels(a => a.userId == userId);
                if (messages.OrderByDescending(a => a.createDate).ToList().Count <= 0) return true;
                var timeSpan = DateTime.Now - messages.OrderByDescending(a => a.createDate).ToList()[0].createDate;
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
            bool flag;
            var user = await UserService.GetUserByIdAsync(t.userId);
            if (await IsExistsMessageOnNewestByUserId(t.userId))
            {
                // 判断用户是否为空
                if (user == null) return false;
                t.location = LocationHelper.GetLocation(t.location);
                flag = await MessageRepository.AddAsync(t);
            }
            else
            {
                throw new Exception("你已经在一分钟前提交过一次了");
            }

            return flag;
        }

        /// <summary>
        /// 删除留言
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveEntityAsync(int tId)
        {
            var message = await MessageRepository.GetSingleModelAsync(a => a.id == tId);
            return await MessageRepository.DeleteAsync(message);
        }

        /// <summary>
        /// 递归调用留言列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<PageDTO<MessageTreeDTO>> GetMessagesByRecursionAsync(int limit, int pageIndex)
        {
            var messageTopEntities = MessageRepository.GetModelsByPage(limit, pageIndex, false, a => a.createDate,
                a => (a.targetId == 0 && a.targetUserId == 0), out var total).ToList();
            var messageTreeList = new List<MessageTreeDTO>();
            foreach (var item in messageTopEntities)
            {
                var messageTree = Mapper.Map<MessageTreeDTO>(item);
                await ConvertMessageTree(messageTree);
                var entities = await MessageRepository.GetMessagesRecursiveAsync(item.id);
                messageTree.children = Mapper.Map<List<MessageTreeDTO>>(entities.ToList());
                messageTreeList.Add(messageTree);
            }

            return new PageDTO<MessageTreeDTO>(pageIndex, limit, total, messageTreeList);
        }

        /// <summary>
        /// 用户信息填充
        /// </summary>
        /// <param name="treeDto"></param>
        private async Task ConvertMessageTree(MessageTreeDTO treeDto)
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

        public async Task<List<MessageDTO>> GetMessageOnNearSaveAsync(int count)
        {
            return await Task.Run(() => MessageRepository.GetMessageOnNearSaveAsync(count));
        }
    }
}