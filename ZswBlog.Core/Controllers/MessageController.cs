using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IMapper _mapper;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="messageService"></param>
        /// <param name="emailHelper"></param>
        /// <param name="mapper"></param>
        public MessageController(IMessageService messageService, EmailHelper emailHelper, IMapper mapper)
        {
            _messageService = messageService;
            _emailHelper = emailHelper;
            _mapper = mapper;
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
            param.createDate = DateTime.Now;
            param.targetId ??= 0;
            param.targetUserId ??= 0;
            HttpContext context = this.HttpContext;
            param.ip = context.Request.Headers["X-Forwarded-For"];
            var flag = await _messageService.AddMessageAsync(param);
            // 发送邮件
            if (param.targetId == 0 || param.targetUserId == null) return Ok(flag);
            var toMessage = await _messageService.GetMessageByIdAsync(param.targetId.Value);
            var fromMessage = await _messageService.GetMessageByIdAsync(param.id);
            var isSendReplyEmail = await _emailHelper.ReplySendEmailAsync(toMessage, fromMessage, SendEmailType.回复留言);
            flag = isSendReplyEmail;
            return Ok(flag);
        }

        /// <summary>
        /// 获取留言列表
        /// </summary>
        /// <param name="limit">获取数</param>
        /// <param name="pageIndex">分页页码</param>
        /// <returns></returns>
        [Route("/api/message/admin/get/page")]
        [HttpGet]
        [Authorize]
        [FunctionDescription("后台管理-分页获取留言列表")]
        public async Task<ActionResult<PageDTO<MessageDTO>>> GetMessageListByPage([FromQuery] int limit, [FromQuery] int pageIndex)
        {
            var pageDto = await _messageService.GetAllMessageListByPageAsync(limit, pageIndex);
            return Ok(pageDto);
        }
        /// <summary>
        /// 删除留言
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [Route("/api/message/admin/remove/{id}")]
        [Authorize]
        [HttpDelete]
        [FunctionDescription("后台管理-删除留言")]
        public ActionResult<bool> RemoveMessage([FromRoute] int id)
        {
            var flag = _messageService.RemoveMessageById(id);
            return Ok(flag);
        }

        /// <summary>
        /// 更新留言
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="isShow">是否显示</param>
        /// <returns></returns>
        [Route("/api/message/admin/update")]
        [Authorize]
        [HttpPost]
        [FunctionDescription("后台管理-更新留言")]
        public async Task<ActionResult<bool>> UpdateMessage([FromQuery] int id, [FromQuery] bool isShow)
        {
            MessageDTO message = await _messageService.GetMessageByIdAsync(id);
            MessageEntity messageEntity = _mapper.Map<MessageEntity>(message);
            messageEntity.isShow = isShow;
            var flag = _messageService.UpdateEntity(messageEntity);
            return Ok(flag);
        }

    }
}