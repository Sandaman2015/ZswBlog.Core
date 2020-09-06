
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface IMessageService : IBaseService<Message>
    {
        /// <summary>
        /// 根据留言Id获取留言
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        Task<Message> GetMessageByIdAsync(int messageId);
        /// <summary>
        /// 获取所有留言
        /// </summary>
        /// <returns></returns>
        Task<List<Message>> GetAllMessagesAsync();
        /// <summary>
        /// 根据父留言Id获取所有父留言下的子留言
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        Task<List<Message>> GetMessagesByTargetIdAsync(int targetId);
        /// <summary>
        /// 分页获取留言
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        Task<List<Message>> GetMessagesByPageAsync(int limit, int pageIndex);
        /// <summary>
        /// 获取所有的单独的留言
        /// </summary>
        /// <returns></returns>
        Task<List<Message>> GetMessagesOnNotReplyAsync();
        /// <summary>
        /// 分页获取单独的留言
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        Task<List<Message>> GetMessagesOnNotReplyAsyncByPageAsync(int limit, int pageIndex);
        /// <summary>
        /// 根据用户获取最新提交的留言
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsExistsMessageOnNewestByUserId(Guid userId);
        /// <summary>
        /// 递归获取所有留言
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        List<Message> GetMessagesByRecursion(int targetId);
        /// <summary>
        /// 清空所有留言
        /// </summary>
        /// <returns></returns>
        bool ClearRecursionMessages();
    }
}
