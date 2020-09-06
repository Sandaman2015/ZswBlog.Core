using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class MessageRepository : BasicRepository<Message>, IMessageRepository, IBaseRepository<Message>
    {

    }
}
