
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface IMessageService : IBaseService<MessageEntity>
    {
        /// <summary>
        /// 根据留言Id获取留言
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        MessageDTO GetMessageById(int messageId);

        /// <summary>
        /// 分页获取留言
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        PageDTO<MessageTreeDTO> GetMessagesByRecursion(int limit, int pageIndex);

        /// <summary>
        /// 根据count获取顶级留言
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        List<MessageDTO> GetMessageOnNoReplyAndCount(int count);

        /// <summary>
        /// 获取最近添加的留言列表
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        List<MessageDTO> GetMessageOnNearSave(int count);

        /// <summary>
        /// 根据用户获取最新提交的留言
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool IsExistsMessageOnNewestByUserId(int userId);
        bool AddMessage(MessageEntity t);
    }
}
