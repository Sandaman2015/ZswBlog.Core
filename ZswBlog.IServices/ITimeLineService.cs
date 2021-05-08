using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface ITimeLineService : IBaseService<TimeLineEntity>
    {
        /// <summary>
        /// 获取所有的时间线按照时间排序
        /// </summary>
        /// <returns></returns>
        Task<List<TimeLineDTO>> GetTimeLineListAsync();
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> RemoveEntityAsync(int id);
    }
}
