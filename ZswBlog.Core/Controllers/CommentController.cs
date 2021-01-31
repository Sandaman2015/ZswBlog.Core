using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.Common;
using ZswBlog.Core.config;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;
using ZswBlog.ThirdParty.Email;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 评论控制器
    /// </summary>
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly EmailHelper _emailHelper;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="commentService"></param>
        /// <param name="emailHelper"></param>
        public CommentController(ICommentService commentService, EmailHelper emailHelper)
        {
            _commentService = commentService;
            _emailHelper = emailHelper;
        }


        /// <summary>
        /// 分页获取文章评论列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [Route(template: "/api/comment/get/page")]
        [HttpGet]
        [FunctionDescription("分页获取文章评论列表")]
        public async Task<ActionResult<PageDTO<CommentTreeDTO>>> GetCommentTreeListByPage([FromQuery] int limit,
            [FromQuery] int pageIndex, [FromQuery] int articleId)
        {
            var pageDto = await _commentService.GetCommentsByRecursionAsync(limit, pageIndex, articleId);
            return Ok(pageDto);
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Route(template: "/api/comment/save")]
        [HttpPost]
        [FunctionDescription("添加文章评论")]
        public async Task<ActionResult> SaveMessage([FromBody] CommentEntity param)
        {
            // 获取IP地址
            if (param.location != null)
            {
                if (Request.HttpContext.Connection.RemoteIpAddress != null)
                {
                    var ip = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                    param.location = ip;
                }
            }

            param.createDate = DateTime.Now;
            param.targetId ??= 0;
            var flag = await _commentService.AddCommentAsync(param);
            // 发送邮件
            if (param.targetId == 0 || param.targetUserId == null) return Ok(flag);
            var toComment = await _commentService.GetCommentByIdAsync(param.targetId.Value);
            var fromComment = await _commentService.GetCommentByIdAsync(param.id);
            flag = _emailHelper.ReplySendEmail(toComment, fromComment, SendEmailType.回复评论);
            return Ok(flag);
        }
    }
}