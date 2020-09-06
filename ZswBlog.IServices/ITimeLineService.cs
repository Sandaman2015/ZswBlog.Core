using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface ITimeLineService : IBaseService<Timeline>
    {
        /// <summary>
        /// 获取所有的时间线
        /// </summary>
        /// <returns></returns>
        Task<List<Timeline>> GetAllTimeLinesOrderByTimeAsync();
    }
}
