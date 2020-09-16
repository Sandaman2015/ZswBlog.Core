using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class MessageService : BaseService<MessageEntity, IMessageRepository>, IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

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
        /// 用户上次留言时间
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
            int pageCount;
            List<MessageEntity> messages = _messageRepository.GetModelsByPage(limit, pageIndex, false, (a => a.createDate), (a => a.id != 0), out pageCount).ToList();
            List<MessageTreeDTO> messageDTOs = _mapper.Map<List<MessageTreeDTO>>(messages);
            foreach (MessageTreeDTO messageTree in messageDTOs) {
                RecursionComments(messageTree, messageTree.id);
            }
            return new PageDTO<MessageTreeDTO>(pageIndex, limit, pageCount, messageDTOs);
        }

        /// <summary>
        /// 清空列表
        /// </summary>
        /// <returns></returns>
        private bool RecursionComments(MessageTreeDTO treeDTO, int targetId)
        {
            List<MessageEntity> messages = _messageRepository.GetModels((MessageEntity me) => me.targetId == targetId).ToList();
            if (messages.Count > 0)
            {
                List<MessageTreeDTO> messageTrees = _mapper.Map<List<MessageTreeDTO>>(messages);
                foreach (MessageTreeDTO message in messageTrees)
                {
                    treeDTO.children.Add(message);
                    return RecursionComments(treeDTO, message.id);
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 根据count获取顶级留言
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<MessageDTO> GetMessageOnNoReplyAndCount(int count)
        {
            List<MessageEntity> messages = _messageRepository.GetModels(a => a.targetId == null).Take(count).ToList();
            return _mapper.Map<List<MessageDTO>>(messages);
        }
    }
}
