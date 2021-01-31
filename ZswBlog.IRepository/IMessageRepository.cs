using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IRepository
{
    public interface IMessageRepository : IBaseRepository<MessageEntity> {
        Task<List<MessageDTO>> GetMessageOnNoReplyAsync(int count);
        Task<List<MessageDTO>> GetMessagesRecursiveAsync(int targetId);
        Task<List<MessageDTO>> GetMessageOnNearSaveAsync(int count);
    }
}
