
using System;
using System.Threading.Tasks;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace Services
{
    public class FileAttachmentService : BaseService, IFileAttachmentService
    {
        private readonly IFileAttachmentRepository _repository;

        public FileAttachmentService(IFileAttachmentRepository repository)
        {
            _repository = repository;
        }
        public async Task<string> GetFilePathById(Guid id)
        {
            return await Task.Run(() =>
            {
                string path = _repository.GetSingleModel(a => a.ID == id).Path;
                return path;
            });
        }
    }
}
