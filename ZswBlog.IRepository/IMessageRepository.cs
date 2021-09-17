using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IRepository
{
    public interface IMessageRepository : IBaseRepository<MessageEntity> {
        List<MessageDTO> GetMessageOnNoReply(int count);
        List<MessageDTO> GetMessagesRecursive(int targetId);
        List<MessageDTO> GetMessageOnNearSave(int count);
    }
}
