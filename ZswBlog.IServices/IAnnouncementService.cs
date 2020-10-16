using System;
using System.Collections.Generic;
using System.Text;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface IAnnouncementService : IBaseService<AnnouncementEntity>
    {
        List<AnnouncementDTO> GetAnnouncementsOnTop(int count);
        List<AnnouncementDTO> GetPushAnnouncement();
        List<AnnouncementDTO> GetAllAnnouncement();
    }
}
