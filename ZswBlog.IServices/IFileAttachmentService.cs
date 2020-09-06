using System;
using System.Threading.Tasks;

namespace ZswBlog.IServices
{
    public interface IFileAttachmentService
    {
        public Task<string> GetFilePathById(Guid id);
    }
}
