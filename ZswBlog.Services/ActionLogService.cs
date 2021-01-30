using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZswBlog.Common;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public class ActionLogService : BaseService<ActionLogEntity, IActionLogRepository>, IActionLogService
    {
        public IActionLogRepository ActionLogRepository { get; set; }

        /// <summary>
        /// 分页获取操作记录
        /// </summary>
        /// <param name="limit">页码</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="logType"></param>
        /// <param name="dimTitle"></param>
        /// <returns></returns>
        public async Task<PageDTO<ActionLogEntity>> GetActionListByPage(int limit, int pageIndex, int logType, string dimTitle)
        {
            return await Task.Run(() =>
            {
                Expression<Func<ActionLogEntity, bool>> expression = t => true;
                if (logType != 0)
                {
                    expression = expression.And(a => a.logType == logType);
                }
                if (!string.IsNullOrEmpty(dimTitle))
                {
                    expression = expression.And(a => a.actionDetail.Contains(dimTitle));
                }
                var actionLogList = ActionLogRepository.GetModelsByPage(limit, pageIndex, true,
                    a => a.createDate, expression,
                    out var pageCount).ToList();
                return new PageDTO<ActionLogEntity>(limit,
                    pageIndex,
                    pageCount,
                    actionLogList);
            });
        }

        /// <summary>
        /// 获取操作日志记录
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<ActionLogEntity> GetActionLogById(int id)
        {
            return await ActionLogRepository.GetSingleModelAsync(a => a.id == id);
        }
    }
}