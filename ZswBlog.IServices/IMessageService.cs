
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
        Task<MessageDTO> GetMessageByIdAsync(int messageId);

        /// <summary>
        /// 分页获取留言
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        Task<PageDTO<MessageTreeDTO>> GetMessagesByRecursionAsync(int limit, int pageIndex);

        /// <summary>
        /// 获取最近添加的留言列表
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<MessageDTO>> GetMessageOnNearSaveAsync(int count);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<bool> AddMessageAsync(MessageEntity t);
    }
}
