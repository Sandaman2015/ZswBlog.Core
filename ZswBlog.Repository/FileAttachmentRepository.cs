using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class FileAttachmentRepository : BaseRepository<FileAttachmentEntity>, IFileAttachmentRepository, IBaseRepository<FileAttachmentEntity> { }
}
