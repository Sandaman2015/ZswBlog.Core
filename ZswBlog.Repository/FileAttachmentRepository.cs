using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class FileAttachmentRepository : BasicRepository<FileAttachment>, IFileAttachmentRepository, IBaseRepository<FileAttachment> { }
}
