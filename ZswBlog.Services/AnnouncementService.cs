using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZswBlog.DTO;
using ZswBlog.Entity;
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
        public List<AnnouncementDTO> GetAllAnnouncement()
        {
            List<AnnouncementEntity> announcements = _announcementRepository.GetModels(a => a.id != 0).ToList();
            return _mapper.Map<List<AnnouncementDTO>>(announcements);
        }

        /// <summary>
        /// 获取指定的置顶通知公告
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<AnnouncementDTO> GetAnnouncementsOnTop(int count)
        {
            List<AnnouncementEntity> announcements = _announcementRepository.GetModels(a => a.isTop && a.endPushDate < DateTime.Now).Take(count).ToList();
            return _mapper.Map<List<AnnouncementDTO>>(announcements);
        }

        /// <summary>
        /// 获取正在推送的通知公告
        /// </summary>
        /// <returns></returns>
        public List<AnnouncementDTO> GetPushAnnouncement()
        {
            List<AnnouncementEntity> announcements = _announcementRepository.GetModels(a => a.endPushDate > DateTime.Now).ToList();
            return _mapper.Map<List<AnnouncementDTO>>(announcements);
        }
    }
}
