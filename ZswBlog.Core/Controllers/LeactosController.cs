//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ZswBlog.DTO;
//using ZswBlog.Entity;
//using ZswBlog.IServices;
//using ZswBlog.MapperFactory;
//using ZswBlog.ThirdParty;

//namespace ZswBlog.Web.Controllers
//{
//    /// <summary>
//    /// 留言页
//    /// </summary>
//    [Route("/api/[controller]/[action]")]
//    [ApiController]
//    public class LeactosController : ControllerBase
//    {
//        private readonly IMessageService messageService;
//        private readonly IUserService userService;
//        private readonly MessageMapper mapperToMessageDTO;
//        private readonly EmailHelper sendEmail;

//#pragma warning disable CS1591 // 缺少对公共可见类型或成员“LeactosController.LeactosController(IMessageService, IUserService, MessageMapper, EmailHelper)”的 XML 注释
//        public LeactosController(IMessageService messageService, IUserService userService, MessageMapper mapperToMessageDTO, EmailHelper sendEmail)
//#pragma warning restore CS1591 // 缺少对公共可见类型或成员“LeactosController.LeactosController(IMessageService, IUserService, MessageMapper, EmailHelper)”的 XML 注释
//        {
//            this.messageService = messageService;
//            this.userService = userService;
//            this.mapperToMessageDTO = mapperToMessageDTO;
//            this.sendEmail = sendEmail;
//        }

//        /// <summary>
//        /// 分页获取留言列表
//        /// </summary>
//        /// <param name="limit"></param>
//        /// <param name="pageIndex"></param>
//        /// <returns></returns>
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<MessageAggregationDTO>>> GetMessagesByPage(int limit, int pageIndex)
//        {
//            List<Message> messages = (await messageService.GetMessagesOnNotReplyAsyncByPageAsync(limit, pageIndex)).ToList();
//            List<MessageAggregationDTO> messagesDTOs = new List<MessageAggregationDTO>();
//            foreach (var item in messages)
//            {
//                MessageAggregationDTO messageAggregationDTO = new MessageAggregationDTO();//父元素
//                User user = await userService.GetUserByIdAsync(item.UserId);
//                MessageDTO messageDTO = await mapperToMessageDTO.MapperToSingleDTOAsync(item, user);
//                //递归查询子留言     
//                messageService.ClearRecursionMessages();
//                List<Message> messagesChildren = messageService.GetMessagesByRecursion(item.MessageId);
//                List<MessageDTO> messageChildrenDTOs = new List<MessageDTO>();//子集元素
//                foreach (var that in messagesChildren)
//                {
//                    User userChildren = await userService.GetUserByIdAsync(that.UserId);
//                    MessageDTO messageChildrenDTO = await mapperToMessageDTO.MapperToSingleDTOAsync(that, userChildren);
//                    messageChildrenDTO.TargetUserName = (userService.GetUserByIdAsync((Guid)that.TargetUserId)).Result.UserName;
//                    messageChildrenDTOs.Add(messageChildrenDTO);
//                }
//                //汇总
//                messageAggregationDTO.MessagesChildren = messageChildrenDTOs.OrderBy(a => a.MessageDate).ToList();
//                messageAggregationDTO.MessageParent = messageDTO;
//                messagesDTOs.Add(messageAggregationDTO);
//            }
//            //获取总数
//            int count = (await messageService.GetMessagesOnNotReplyAsync()).Count();
//            return Ok(new { data = messagesDTOs, total = count });
//        }

//        /// <summary>
//        /// 添加留言
//        /// </summary>
//        /// <param name="param"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public async Task<ActionResult> AddMessage(Message param)
//        {
//            int Code;
//            string str;
//            User user = await userService.GetUserByIdAsync(param.UserId);
//            if (await messageService.IsExistsMessageOnNewestByUserId(param.UserId))
//            {
//                if (user.UserEmail != null && user.UserEmail != "")
//                {
//                    param.Location = LocationHelper.GetLocation(param.Location);//获取位置
//                    param.MessageDate = DateTime.Now;
//                    Code = await messageService.AddEntityAsync(param) ? 200 : 500;

//                    if (param.TargetId != 0 && param.TargetUserId != Guid.Empty && param.TargetUserId != null)
//                    {
//                        bool ok = await sendEmail.ReplySendEmail(await messageService.GetMessageByIdAsync(param.TargetId), await messageService.GetMessageByIdAsync(param.MessageId), SendEmailType.回复留言);//发送邮件
//                        if (ok)
//                        {
//                            str = Code == 200 ? "回复成功！" : "回复失败！请刷新页面后重试！";
//                        }
//                        else
//                        {
//                            str = Code == 200 ? "回复成功,但是该回复并未通知到用户！" : "回复失败！请刷新页面后重试！";
//                        }
//                    }
//                    else
//                    {
//                        str = Code == 200 ? "评论成功！" : "评论失败！请刷新页面后重试！";
//                    };
//                }
//                else
//                {
//                    Code = 401;
//                    str = "您还未填写邮箱，请填写邮箱后再留言，谢谢！";
//                }
//            }
//            else
//            {
//                Code = 400;
//                str = "您在1分钟已经提交过一次了，有什么想说的欢迎直接联系我哦！";
//            }
//            return Ok(new { code = Code, msg = str });
//        }
//    }
//}
