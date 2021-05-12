using AutoMapper;
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
            return await Task.Run(() =>
            {
                var announcements = AnnouncementRepository.GetModels(a => a.id != 0);
                return Mapper.Map<List<AnnouncementDTO>>(announcements.ToList());
            });
        }

        /// <summary>
        /// 分页获取通知公告
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页码</param>
        /// <returns></returns>
        public async Task<PageDTO<AnnouncementDTO>> GetAnnouncementAsyncByPage(int pageIndex, int pageSize)
        {
            return await Task.Run(() =>
            {
                var announcements = AnnouncementRepository.GetModelsByPage(pageSize, pageIndex, true, a => a.createDate,
                    a => a.endPushDate < DateTime.Now, out var total);
                var list = Mapper.Map<List<AnnouncementDTO>>(announcements.ToList());
                return new PageDTO<AnnouncementDTO>(pageIndex, pageSize, total, list);
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
                var announcements =
                 AnnouncementRepository.GetModels(a => a.isTop && a.endPushDate < DateTime.Now
                                                                         && a.isShow
                );
                return Mapper.Map<List<AnnouncementDTO>>(announcements.Take(count).ToList());
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
                var announcements =  AnnouncementRepository.GetModels(a => a.endPushDate > DateTime.Now
                && a.isShow);
                return Mapper.Map<List<AnnouncementDTO>>(announcements.ToList());
            });
        }
    }
}