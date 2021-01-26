using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;

namespace ZswBlog.IRepository
{
    public interface ICommentRepository : IBaseRepository<CommentEntity> {
        Task<List<CommentDTO>> GetCommentsRecursiveAsync(int targetId, int articleId);
    }
}
