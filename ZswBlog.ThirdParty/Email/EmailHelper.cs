﻿using System.Net.Mail;
using System.Text;
using ZswBlog.Entity;
using ZswBlog.IServices;
using ZswBlog.Util;

namespace ZswBlog.ThirdParty.Email
{
    public enum SendEmailType
    {
        回复留言,
        回复评论
    }
    public class EmailHelper
    {
        private readonly IMessageService _messageService;
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private string _sendEmailAddress;
        private string _emailSecretKey;
        public EmailHelper(IMessageService messageService, ICommentService commentService, IUserService userService)
        {
            _messageService = messageService;
            _commentService = commentService;
            _userService = userService;
            this._sendEmailAddress = ConfigHelper.GetValue("SendEmailAddress");
            this._emailSecretKey = ConfigHelper.GetValue("EmailSecretKey");
        }
        //TODO 发送邮件需要分离
        /// <summary>
        /// 1.0的发送邮件
        /// </summary>
        /// <param name="targetUser"></param>
        /// <param name="user"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool ReplySendEmail(dynamic toUser, dynamic fromUser, SendEmailType sendEmailType)
        {
            
            MailAddress MessageFrom = new MailAddress(_sendEmailAddress); //发件人邮箱地址 
            User targetUser = _userService.GetUserById(toUser.UserId);
            User user = _userService.GetUserById(fromUser.UserId);
            string MessageTo = targetUser.UserEmail; //收件人邮箱地址 
            string MessageSubject = "ZswBlog博客回复通知"; //邮件主题    
            string Content = null;//目标的内容
            string ReplyContent = null;//回复的内容
            string url = null;//目标地址

            if (sendEmailType == SendEmailType.回复评论)
            {
                //评论回复
                var replyMessage = _commentService.GetCommentById(fromUser.CommentId);
                ReplyContent = replyMessage.Comment1;
                var message = _commentService.GetCommentById(toUser.CommentId);
                Content = message.Comment1;
                int ArticleId = message.ArticleId;
                url = "https://www.zswblog.xyz/details.html?ArticleDetails=" + ArticleId + "";
            }
            else
            {
                //留言回复
                var replyLeacots = _messageService.GetMessageById(fromUser.MessageId);
                ReplyContent = replyLeacots.Message1;
                var leacots = _messageService.GetMessageById(toUser.MessageId);
                Content = leacots.Message1;
                url = "https://www.zswblog.xyz/leacots.html";
            }
            //邮件内容
            string MessageBody = "<div id=\"contentDiv\" onmouseover=\"getTop().stopPropagation(event);\" onclick=\"getTop().preSwapLink(event, 'spam', 'ZC3011-yZb5lAAS2SKCSSF8palnY9a');";
            MessageBody += "style=\"position: relative; font - size:14px; height: auto; padding: 15px 15px 10px 15px; z - index:1; zoom: 1; line - height:1.7; \"";
            MessageBody += "class=\"body\"><div id=\"qm_con_body\"><div id=\"mailContentContainer\" class=\"qmbox qm_con_body_content qqmail_webmail_only\" >";
            MessageBody += "<div style=\"position: relative; color:#555;font:12px/1.5 Microsoft YaHei,Tahoma,Helvetica,Arial,sans-serif;max-width:600px;margin:50px auto;border-radius: 5px;box-shadow:0 5px 10px #aaaaaa;background: 0 0 repeat-x #FFF;background-image: -webkit-repeating-linear-gradient(135deg, #4882CE, #4882CE 20px, #FFF 20px, #FFF 35px, #EB1B2E 35px, #EB1B2E 55px, #FFF 55px, #FFF 70px);background-image: repeating-linear-gradient(-45deg, #4882CE, #4882CE 20px, #FFF 20px, #FFF 35px, #EB1B2E 35px, #EB1B2E 55px, #FFF 55px, #FFF 70px);background-size: 100% 10px\">";
            MessageBody += "<div style=\"padding: 0 15px 8px;\"><h2 style=\"border-bottom:1px solid #e9e9e9;font-size:18px;font-weight:normal;padding:20px 0 10px;\"><span style=\"color: #12ADDB\">&gt;";
            MessageBody += "</span>您在 <a style=\"text - decoration:none; color: #12ADDB;\" href=\"https://www.zswblog.xyz\" target=\"_blank\" rel=\"noopener\">张晟玮博客</a> 中的评论或留言有新回复啦！</h2><div style=\"font-size:14px;color:#777;padding:0 10px;margin-top:18px\">";
            MessageBody += "<p>" + targetUser.UserName + "，您曾在张晟玮博客中发表评论或留言：</p><div style=\"background-color:#f5f5f5;padding:10px 15px;margin:18px 0;\">" + Content + "</div><p>" + user.UserName + "回复说:</p><div style=\"background-color:#f5f5f5;padding: 10px 15px;margin:18px 0;\">" + ReplyContent + "</div>";
            MessageBody += "<p>您可以点击<a style=\"text - decoration:none; color:#12addb\" href=" + url + " title=\"点击查看完整的回复内容\" target=\"_blank\" rel=\"noopener\">查看完整的回复內容</a>,欢迎回访<a style=\"text - decoration:none; color:#12addb\" href=\"https://www.zswblog.xyz\" target=\"_blank\" rel=\"noopener\">张晟玮的博客</a>！</p>";
            MessageBody += "</div></div><div style=\"color:#888;padding:10px;border-top:1px solid #e9e9e9;background:#f5f5f5;border-radius: 0 0 5px 5px;\">";
            MessageBody += "<p style=\"margin: 0; padding: 0; \">Copyright ©<span style=\"border - bottom - width: 1px; border - bottom - style: dashed; border - bottom - color: rgb(204, 204, 204); z - index: 1; position: static; \"><span";
            MessageBody += "style=\"border - bottom:1px dashed #ccc;z-index:1\" t=\"7\" onclick=\"return false;\" data=\"2019-2019\">2019-2019</span>";
            MessageBody += "</span><a style=\"color:#888;text-decoration:none;\" href=\"https://www.zswblog.xyz\" target=\"_blank\" rel=\"noopener\">张晟玮的博客</a>- 本邮件自动生成，请勿直接回复！</p>";
            MessageBody += "</div></div><style type=\"text/css\">.qmbox style,.qmbox script,.qmbox head,.qmbox link,.qmbox meta {display: none !important;}</style></div></div>";
            MessageBody += "<style>#mailContentContainer .txt {height: auto;}</style></div>";
            return SendMail(MessageFrom, MessageTo, MessageSubject, MessageBody);

        }

        public bool SendMail(MailAddress MessageFrom, string MessageTo, string MessageSubject, string MessageBody)   //发送验证邮件
        {
            MailMessage message = new MailMessage();
            message.To.Add(MessageTo);
            message.From = MessageFrom;
            message.Subject = MessageSubject;
            message.SubjectEncoding = Encoding.UTF8;
            message.Body = MessageBody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true; //是否为html格式 
            message.Priority = MailPriority.High; //发送邮件的优先等级 
            SmtpClient sc = new SmtpClient();
            sc.EnableSsl = true;//是否SSL加密
            sc.Host = "smtp.qq.com"; //指定发送邮件的服务器地址或IP 
            sc.Port = 587; //指定发送邮件端口 
            sc.Credentials = new System.Net.NetworkCredential(_sendEmailAddress, _emailSecretKey); 
            sc.Send(message); //发送邮件 
                              //_log.Debug("发送邮件到" + MessageTo + "成功");
            return true;

        }
    }
}
