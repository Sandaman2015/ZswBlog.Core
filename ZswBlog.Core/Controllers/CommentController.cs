using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMapper _mapper;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="commentService"></param>
        /// <param name="emailHelper"></param>
        /// <param name="mapper"></param>
        public CommentController(ICommentService commentService, EmailHelper emailHelper, IMapper mapper)
        {
            _commentService = commentService;
            _emailHelper = emailHelper;
            _mapper = mapper;
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
            param.createDate = DateTime.Now;
            param.targetId ??= 0;
            var flag = await _commentService.AddCommentAsync(param);
            // 发送邮件
            if (param.targetId == 0 || param.targetUserId == null) return Ok(flag);
            var toComment = await _commentService.GetCommentByIdAsync(param.targetId.Value);
            var fromComment = await _commentService.GetCommentByIdAsync(param.id);
            flag = await _emailHelper.ReplySendEmailAsync(toComment, fromComment, SendEmailType.回复评论);
            return Ok(flag);
        }

        /// <summary>
        /// 获取留言列表
        /// </summary>
        /// <param name="limit">获取数</param>
        /// <param name="pageIndex">分页页码</param>
        /// <returns></returns>
        [Route("/api/comment/admin/get/page")]
        [HttpGet]
        [Authorize]
        [FunctionDescription("后台管理-分页获取评论列表")]
        public async Task<ActionResult<PageDTO<CommentDTO>>> GetCommentListByPage([FromQuery] int limit, [FromQuery] int pageIndex)
        {
            var pageDto = await _commentService.GetAllCommentListByPageAsync(limit, pageIndex);
            return Ok(pageDto);
        }


        /// <summary>
        /// 删除留言
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [Route("/api/comment/admin/remove/{id}")]
        [Authorize]
        [HttpDelete]
        [FunctionDescription("后台管理-删除留言")]
        public ActionResult<bool> RemoveComment([FromRoute] int id)
        {
            var flag = _commentService.RemoveCommentById(id);
            return Ok(flag);
        }

        /// <summary>
        /// 更新评论
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="isShow">是否显示</param>
        /// <returns></returns>
        [Route("/api/comment/admin/update")]
        [Authorize]
        [HttpPost]
        [FunctionDescription("后台管理-更新评论展示")]
        public async Task<ActionResult<bool>> UpdateComment([FromQuery] int id, [FromQuery] bool isShow)
        {
            CommentDTO comment = await _commentService.GetCommentByIdAsync(id);
            CommentEntity commentEntity = _mapper.Map<CommentEntity>(comment);
            commentEntity.isShow = isShow;
            var flag = _commentService.UpdateEntity(commentEntity);
            return Ok(flag);
        }
    }
}