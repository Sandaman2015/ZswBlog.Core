﻿using AutoMapper;
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
    public class MessageService : BaseService<MessageEntity, IMessageRepository>, IMessageService
    {
        public IMessageRepository _messageRepository { get; set; }
        public IMapper _mapper { get; set; }
        public IUserService _userService { get; set; }
        /// <summary>
        /// 获取留言详情
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public MessageDTO GetMessageById(int messageId)
        {
            MessageEntity message = _messageRepository.GetSingleModel(a => a.id == messageId);
            return _mapper.Map<MessageDTO>(message);
        }

        /// <summary>
        /// 用户上次留言时间是否大于1分钟
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsExistsMessageOnNewestByUserId(int userId)
        {
            List<MessageEntity> messages = _messageRepository.GetModels(a => a.userId == userId).OrderByDescending(a => a.createDate).ToList();
            if (messages != null && messages.Count > 0)
            {
                TimeSpan timeSpan = DateTime.Now - messages[0].createDate;
                return timeSpan.TotalMinutes > 1;
            }
            else return true;
        }

        /// <summary>
        /// 添加留言
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool AddMessage(MessageEntity t)
        {
            bool flag = false;
            UserDTO user = _userService.GetUserById(t.userId);
            if (IsExistsMessageOnNewestByUserId(t.userId))
            {
                // 判断用户是否为空
                if (user != null)
                {
                    t.location = LocationHelper.GetLocation(t.location);
                    flag = _messageRepository.Add(t);
                }
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
        public bool RemoveEntity(int tId)
        {
            MessageEntity message = _messageRepository.GetSingleModel(a => a.id == tId);
            return _messageRepository.Delete(message);
        }

        /// <summary>
        /// 递归调用留言列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public PageDTO<MessageTreeDTO> GetMessagesByRecursion(int limit, int pageIndex)
        {
            List<MessageEntity> messageTopEntities = _messageRepository.GetModelsByPage(limit, pageIndex, false, a => a.createDate, a => a.targetId == 0 && a.targetUserId == null, out int total).ToList();
            List<MessageTreeDTO> messageTreeList = new List<MessageTreeDTO>();
            foreach (var item in messageTopEntities)
            {
                MessageTreeDTO messageTree = _mapper.Map<MessageTreeDTO>(item);
                ConvertMessageTree(messageTree);
                List<MessageDTO> entities = _messageRepository.GetMessagesRecursive(item.id);
                messageTree.children = _mapper.Map<List<MessageTreeDTO>>(entities);
                messageTreeList.Add(messageTree);
            }
            return new PageDTO<MessageTreeDTO>(pageIndex, limit, total, messageTreeList);
        }
        /// <summary>
        /// 根据count获取顶级留言
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<MessageDTO> GetMessageOnNoReplyAndCount(int count)
        {
            List<MessageDTO> messages = _messageRepository.GetMessageOnNoReply(count);
            return messages;
        }

        /// <summary>
        /// 用户信息填充
        /// </summary>
        /// <param name="treeDTO"></param>
        public void ConvertMessageTree(MessageTreeDTO treeDTO)
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

        public List<MessageDTO> GetMessageOnNearSave(int count)
        {
            return _messageRepository.GetMessageOnNearSave(count);
        }
    }
}
