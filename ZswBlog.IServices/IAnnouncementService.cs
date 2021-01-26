using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;

namespace ZswBlog.IServices
{
    public interface IAnnouncementService : IBaseService<AnnouncementEntity>
    {
        Task<List<AnnouncementDTO>> GetAnnouncementsOnTopAsync(int count);
        Task<List<AnnouncementDTO>> GetPushAnnouncementAsync();
        Task<List<AnnouncementDTO>> GetAllAnnouncementAsync();
    }
}
