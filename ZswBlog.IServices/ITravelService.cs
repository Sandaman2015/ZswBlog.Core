using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface ITravelService : IBaseService<Travel>
    {
        /// <summary>
        /// 获取所有的旅行分享信息
        /// </summary>
        /// <returns></returns>
        Task<List<Travel>> GetTravelsAsync();

        /// <summary>
        /// 根据id获取旅行信息
        /// </summary>
        /// <param name="tId">旅行id</param>
        /// <returns></returns>
        Task<Travel> GetTravel(int tId);
    }
}
