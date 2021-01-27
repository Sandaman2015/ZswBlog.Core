using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class AnnouncementService : BaseService<AnnouncementEntity, IAnnouncementRepository>, IAnnouncementService
    {
        public IAnnouncementRepository AnnouncementRepository { get; set; }
        public IMapper Mapper { get; set; }

        /// <summary>
        /// 获取所有通知公告
        /// </summary>
        /// <returns></returns>
        public async Task<List<AnnouncementDTO>> GetAllAnnouncementAsync()
        {
            var announcements = await AnnouncementRepository.GetModelsAsync(a => a.id != 0);
            return Mapper.Map<List<AnnouncementDTO>>(announcements.ToList());
        }

        /// <summary>
        /// 获取指定的置顶通知公告
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<List<AnnouncementDTO>> GetAnnouncementsOnTopAsync(int count)
        {
            var announcements =
                await AnnouncementRepository.GetModelsAsync(a => a.isTop && a.endPushDate < DateTime.Now);
            return Mapper.Map<List<AnnouncementDTO>>(announcements.Take(count).ToList());
        }

        /// <summary>
        /// 获取正在推送的通知公告
        /// </summary>
        /// <returns></returns>
        public async Task<List<AnnouncementDTO>> GetPushAnnouncementAsync()
        {
            var announcements = await AnnouncementRepository.GetModelsAsync(a => a.endPushDate > DateTime.Now);
            return Mapper.Map<List<AnnouncementDTO>>(announcements.ToList());
        }
    }
}