using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface IAnnouncementService : IBaseService<AnnouncementEntity>
    {
        /// <summary>
        /// 获取置顶通知公告
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<AnnouncementDTO>> GetAnnouncementsOnTopAsync(int count);
        /// <summary>
        /// 获取正在推送的通知公告
        /// </summary>
        /// <returns></returns>
        Task<List<AnnouncementDTO>> GetPushAnnouncementAsync();
        /// <summary>
        /// 获取所有通知公告 
        /// </summary>
        /// <returns></returns>
        Task<List<AnnouncementDTO>> GetAllAnnouncementAsync();
        /// <summary>
        /// 分页获取通知公告
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PageDTO<AnnouncementDTO>> GetAnnouncementAsyncByPage(int pageIndex, int pageSize);
    }
}
