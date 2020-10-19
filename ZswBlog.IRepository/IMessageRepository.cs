using System.Collections.Generic;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IRepository
{
    //public interface IMessageRepository : IBaseRepository<Message> { }
    public interface IMessageRepository : IBaseRepository<MessageEntity> {
        List<MessageDTO> GetMessageOnNoReply(int count);
    }
}
