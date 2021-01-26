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
        public IAnnouncementRepository _announcementRepository { get; set; }
        public IMapper _mapper { get; set; }

        /// <summary>
        /// 获取所有通知公告
        /// </summary>
        /// <returns></returns>
        public async Task<List<AnnouncementDTO>> GetAllAnnouncementAsync()
        {
            return await Task.Run(() =>
            {
                var announcements = _announcementRepository.GetModelsAsync(a => a.id != 0).Result.ToList();
                return _mapper.Map<List<AnnouncementDTO>>(announcements);
            });
        }

        /// <summary>
        /// 获取指定的置顶通知公告
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<List<AnnouncementDTO>> GetAnnouncementsOnTopAsync(int count)
        {
            return await Task.Run(() =>
            {
                var announcements = _announcementRepository.GetModelsAsync(a => a.isTop && a.endPushDate < DateTime.Now)
                    .Result.Take(count).ToList();
                return _mapper.Map<List<AnnouncementDTO>>(announcements);
            });
        }

        /// <summary>
        /// 获取正在推送的通知公告
        /// </summary>
        /// <returns></returns>
        public async Task<List<AnnouncementDTO>> GetPushAnnouncementAsync()
        {
            return await Task.Run(() =>
            {
                var announcements = _announcementRepository.GetModelsAsync(a => a.endPushDate > DateTime.Now).Result
                    .ToList();
                return _mapper.Map<List<AnnouncementDTO>>(announcements);
            });
        }
    }
}