using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.Common;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public class ActionLogController: ControllerBase
    {
        private readonly IActionLogService _actionLogService;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="actionLogService"></param>
        public ActionLogController(IActionLogService actionLogService)
        {
            _actionLogService = actionLogService;
        }

        /// <summary>
        /// 分页获取操作日志
        /// </summary>
        /// <param name="limit">页码</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="logType">日志类型</param>
        /// <param name="dimTitle">模糊标题</param>
        /// <returns></returns>
        [Route("/api/action/admin/get/page")]
        [Authorize]
        [HttpGet]
        [FunctionDescription("后台管理-分页获取操作列表")]
        public async Task<ActionResult<PageDTO<ActionLogEntity>>> GetActionLogByPage(int limit, int pageIndex, int logType, string dimTitle)
        {
            var actionList = await _actionLogService.GetActionListByPage(limit, pageIndex, logType, dimTitle);
            return Ok(actionList);
        }

        /// <summary>
        /// 获取操作日志详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/api/action/admin/get/{id}")]
        [Authorize]
        [HttpGet]
        [FunctionDescription("后台管理-获取操作日志详情")]
        public async Task<ActionResult<ActionLogEntity>> GetActionDetails([FromRoute]int id)
        {
            var actionLog = await _actionLogService.GetActionLogById(id);
            return Ok(actionLog);
        }

    }
}