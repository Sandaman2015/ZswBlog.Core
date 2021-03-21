using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class FileAttachmentService : BaseService<FileAttachmentEntity, IFileAttachmentRepository>,
        IFileAttachmentService
    {
        public IFileAttachmentRepository FileAttachmentRepository { get; set; }

        public ITravelFileAttachmentService TravelFileAttachmentService { get; set; }

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

        public virtual async Task<bool> RemoveAllRelationByAttachmentName(string imgName)
        {
            var list = await FileAttachmentRepository.GetModelsAsync(a => a.fileName == imgName);
            var queryList = await list.ToListAsync();
            foreach (var item in queryList)
            {
                await TravelFileAttachmentService.RemoveAllFileRelationAsync(item.id);
            }
            return await FileAttachmentRepository.DeleteListAsync(queryList);
        }
    }
}