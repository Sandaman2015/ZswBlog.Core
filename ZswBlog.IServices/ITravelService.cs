using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;

namespace ZswBlog.IServices
{
    public interface ITravelService : IBaseService<TravelEntity>
    {
        /// <summary>
        /// 获取所有的旅行分享信息
        /// </summary>
        /// <returns></returns>
        Task<PageDTO<TravelDTO>> GetTravelsByPageAsync(int pageSize, int pageIndex);
        Task<TravelDTO> GetTravelAsync(int tId);
    }
}
