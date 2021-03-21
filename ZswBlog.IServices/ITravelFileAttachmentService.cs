using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface ITravelFileAttachmentService : IBaseService<TravelFileAttachmentEntity>
    {
        /// <summary>
        /// 根据旅行分享编码获取所有文件
        /// </summary>
        /// <param name="travelId"></param>
        /// <returns></returns>
        Task<List<FileAttachmentEntity>> GetTravelFileListByTravelId(int travelId);
        /// <summary>
        /// 根据旅行分享编码删除所有关联
        /// </summary>
        /// <param name="travelId"></param>
        /// <returns></returns>
        Task<bool> RemoveAllTravelRelationAsync(int travelId);
        /// <summary>
        /// 根据文件编码删除所有关联
        /// </summary>
        /// <param name="fileAttachmentId"></param>
        /// <returns></returns>
        Task<bool> RemoveAllFileRelationAsync(int fileAttachmentId);
    }
}
