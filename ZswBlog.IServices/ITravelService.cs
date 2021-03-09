using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface ITravelService : IBaseService<TravelEntity>
    {
        /// <summary>
        /// 获取所有的旅行分享信息
        /// </summary>
        /// <returns></returns>
        Task<PageDTO<TravelDTO>> GetTravelsByPageAsync(int pageSize, int pageIndex, bool isShow);
        /// <summary>
        /// 根据编码获取旅行分享详情
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        Task<TravelDTO> GetTravelByIdAsync(int tId);
        /// <summary>
        /// 删除分享编码
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        Task<bool> RemoveTravelAsync(int tId);
    }
}
