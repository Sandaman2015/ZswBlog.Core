using System;
using System.Collections.Generic;
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
    /// 留言控制器
    /// </summary>
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly EmailHelper _emailHelper;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="messageService"></param>
        /// <param name="emailHelper"></param>
        public MessageController(IMessageService messageService, EmailHelper emailHelper)
        {
            _messageService = messageService;
            _emailHelper = emailHelper;
        }

        /// <summary>
        /// 分页获取留言列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [Route(template: "/api/message/get/page")]
        [HttpGet]
        [FunctionDescription("分页获取留言列表")]
        public async Task<ActionResult<PageDTO<MessageTreeDTO>>> GetMessageTreeListByPage([FromQuery] int limit,
            [FromQuery] int pageIndex)
        {
            var pageDto = await _messageService.GetMessagesByRecursionAsync(limit, pageIndex);
            return Ok(pageDto);
        }

        /// <summary>
        /// 获取留言列表
        /// </summary>
        /// <param name="count">获取数</param>
        /// <returns></returns>
        [Route(template: "/api/message/get/list/{count}")]
        [HttpGet]
        [FunctionDescription("获取留言列表")]
        public async Task<ActionResult<List<MessageDTO>>> GetMessageListByCount([FromRoute] int count)
        {
            var pageDto = await _messageService.GetMessageOnNearSaveAsync(count);
            return Ok(pageDto);
        }

        /// <summary>
        /// 添加留言
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Route(template: "/api/message/save")]
        [HttpPost]
        [FunctionDescription("添加留言")]
        public async Task<ActionResult> SaveMessage([FromBody] MessageEntity param)
        {
            // 获取IP地址
            if (param.location != null)
            {
                if (Request.HttpContext.Connection.RemoteIpAddress != null)
                {
                    var ip = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                    param.location = ip;
                    param.ip = ip;
                }
            }

            param.createDate = DateTime.Now;
            param.targetId ??= 0;
            var flag = await _messageService.AddMessageAsync(param);
            // 发送邮件
            if (param.targetId == 0 || param.targetUserId == null) return Ok(flag);
            var toMessage = await _messageService.GetMessageByIdAsync(param.targetId.Value);
            var fromMessage = await _messageService.GetMessageByIdAsync(param.id);
            var isSendReplyEmail = _emailHelper.ReplySendEmail(toMessage, fromMessage, SendEmailType.回复留言);
            flag = isSendReplyEmail;
            return Ok(flag);
        }
    }
}