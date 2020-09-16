
using System;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class FileAttachmentService : BaseService<FileAttachmentEntity, IFileAttachmentRepository>, IFileAttachmentService
    {
        private readonly IFileAttachmentRepository _fileAttachmentRepository;

        public FileAttachmentService(IFileAttachmentRepository repository)
        {
            _fileAttachmentRepository = repository;
        }

        /// <summary>
        /// 根据id获取附件对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileAttachmentEntity GetAttachmentById(int id)
        {
            return _fileAttachmentRepository.GetSingleModel(a => a.id == id);
        }

        /// <summary>
        /// 根据id获取附件路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetFilePathById(int id)
        {             
            return _fileAttachmentRepository.GetSingleModel(a => a.id == id).path;
        }
    }
}
