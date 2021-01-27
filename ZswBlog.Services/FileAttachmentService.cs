using System;
using System.Threading.Tasks;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class FileAttachmentService : BaseService<FileAttachmentEntity, IFileAttachmentRepository>,
        IFileAttachmentService
    {
        public IFileAttachmentRepository FileAttachmentRepository { get; set; }

        /// <summary>
        /// 根据id获取附件对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FileAttachmentEntity> GetAttachmentByIdAsync(int id)
        {
            return await FileAttachmentRepository.GetSingleModelAsync(a => a.id == id);
        }

        /// <summary>
        /// 根据id获取附件路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetFilePathByIdAsync(int id)
        {
            var file = await FileAttachmentRepository.GetSingleModelAsync(a => a.id == id);
            return file.path;
        }
    }
}