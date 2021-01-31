using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    /// <summary>
    /// 日志记录操作
    /// </summary>
    public interface IActionLogService: IBaseService<ActionLogEntity>
    {
        /// <summary>
        /// 获取操作日志记录
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="logType"></param>
        /// <param name="dimTitle"></param>
        /// <returns></returns>
        Task<PageDTO<ActionLogEntity>> GetActionListByPage(int limit, int pageIndex, int logType, string dimTitle);

        /// <summary>
        /// 获取操作详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ActionLogEntity> GetActionLogById(int id);
    }
}