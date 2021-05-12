using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
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

        public async Task<PageDTO<FileAttachmentEntity>> GetFileAttachmentListByPageAsync(int pageIndex, int pageSize)
        {
            List<FileAttachmentEntity> list = await FileAttachmentRepository.GetModelsByPage(pageSize, pageIndex, true, a=>a.createDate, a=> true, out var total).ToListAsync();
            return new PageDTO<FileAttachmentEntity>(pageIndex, pageSize, total, list);
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
            var list = FileAttachmentRepository.GetModels(a => a.fileName == imgName);
            var queryList = await list.ToListAsync();
            foreach (var item in queryList)
            {
                await TravelFileAttachmentService.RemoveAllFileRelationAsync(item.id);
            }
            return await FileAttachmentRepository.DeleteListAsync(queryList);
        }
    }
}