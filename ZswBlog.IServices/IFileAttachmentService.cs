using System;
using System.Threading.Tasks;
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
        public string GetFilePathById(int id);

        /// <summary>
        /// 根据主键获取附件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileAttachmentEntity GetAttachmentById(int id);
    }
}
