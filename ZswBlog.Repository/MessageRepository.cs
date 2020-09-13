using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class MessageRepository : BaseRepository<MessageEntity>, IMessageRepository, IBaseRepository<MessageEntity>
    {

    }
}
