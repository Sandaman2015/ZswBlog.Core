using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class CommentRepository : BasicRepository<Comment>, ICommentRepository, IBaseRepository<Comment>
    {

    }
}
