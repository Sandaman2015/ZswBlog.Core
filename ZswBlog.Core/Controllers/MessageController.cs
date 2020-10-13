﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;
using ZswBlog.ThirdParty.Email;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 留言控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly EmailHelper _emailHelper;
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
        [Route(template: "/message/get/page")]
        [HttpGet]
        public async Task<ActionResult<PageDTO<MessageTreeDTO>>> GetMessageTreeListByPage([FromQuery] int limit, [FromQuery] int pageIndex)
        {
            return await Task.Run(() =>
            {
                PageDTO<MessageTreeDTO> pageDTO = _messageService.GetMessagesByRecursion(limit, pageIndex);
                return Ok(pageDTO);
            });
        }


        /// <summary>
        /// 添加留言
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Route(template: "/message/save")]
        [HttpPost]
        public async Task<ActionResult> SaveMessage([FromBody] MessageEntity param)
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
                msg = _messageService.AddEntity(param) ? "添加成功" : "添加失败";
                // 发送邮件
                if (param.targetId != 0 && param.targetUserId != null)
                {
                    MessageDTO toMessage = _messageService.GetMessageById(param.targetId.Value);
                    MessageDTO fromMessage = _messageService.GetMessageById(param.id);
                    bool isSendReplyEmail = _emailHelper.ReplySendEmail(toMessage, fromMessage, SendEmailType.回复留言);
                    msg = isSendReplyEmail ? "回复成功" : "回复失败,请刷新页面后重试";
                }
                return Ok(msg);
            });
        }
    }
}