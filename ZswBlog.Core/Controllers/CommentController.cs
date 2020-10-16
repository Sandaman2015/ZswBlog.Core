using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;
using ZswBlog.ThirdParty.Email;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 评论控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {

        private readonly ICommentService _commentService;
        private readonly EmailHelper _emailHelper;

        /// <summary>
        /// 
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
        [Route(template: "/comment/get/page")]
        [HttpGet]
        public async Task<ActionResult<PageDTO<CommentTreeDTO>>> GetCommentTreeListByPage([FromQuery] int limit, [FromQuery] int pageIndex,[FromQuery] int articleId)
        {
            return await Task.Run(() =>
            {
                PageDTO<CommentTreeDTO> pageDTO = _commentService.GetCommentsByRecursion(limit, pageIndex, articleId);
                return Ok(pageDTO);
            });
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Route(template: "/comment/save")]
        [HttpPost]
        public async Task<ActionResult> SaveMessage([FromBody] CommentEntity param)
        {
            return await Task.Run(() =>
            {
                string msg;
                // 获取IP地址
                if (param.location != null)
                {
                    string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    param.location = ip;
                }
                msg = _commentService.AddEntity(param) ? "添加成功" : "添加失败";
                // 发送邮件
                if (param.targetId != 0 && param.targetUserId != null)
                {
                    CommentDTO toComment = _commentService.GetCommentById(param.targetId.Value);
                    CommentDTO fromComment = _commentService.GetCommentById(param.id);
                    bool isSendReplyEmail = _emailHelper.ReplySendEmail(toComment, fromComment, SendEmailType.回复评论);
                    msg = isSendReplyEmail ? "回复成功" : "回复失败,请刷新页面后重试";
                }
                return Ok(msg);
            });
        }
    }
}
