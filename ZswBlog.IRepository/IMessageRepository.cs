using System.Collections.Generic;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;

namespace ZswBlog.IRepository
{
    //public interface IMessageRepository : IBaseRepository<Message> { }
    public interface IMessageRepository : IBaseRepository<MessageEntity> {
        List<MessageDTO> GetMessageOnNoReply(int count);
        List<MessageDTO> GetMessagesRecursive(int targetId);
        List<MessageDTO> GetMessageOnNearSave(int count);
    }
}
