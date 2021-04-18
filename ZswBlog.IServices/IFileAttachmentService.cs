using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface IFileAttachmentService : IBaseService<FileAttachmentEntity>
    {
        /// <summary>
        /// 根据主键获取附件路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> GetFilePathByIdAsync(int id);

        /// <summary>
        /// 获取所有图片列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageDTO<FileAttachmentEntity>> GetFileAttachmentListByPageAsync(int pageIndex, int pageSize);

        /// <summary>
        /// 根据主键获取附件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<FileAttachmentEntity> GetAttachmentByIdAsync(int id);

        /// <summary>
        /// 移除所有关联
        /// </summary>
        /// <param name="imgName"></param>
        /// <returns></returns>
        Task<bool> RemoveAllRelationByAttachmentName(string imgName);
        
    }
}
