using System.Net.Mail;
using System.Text;
using ZswBlog.DTO;
using ZswBlog.IServices;
using ZswBlog.Common.Util;

namespace ZswBlog.ThirdParty.Email
{
    public enum SendEmailType
    {
        回复留言,
        回复评论
    }
    /// <summary>
    /// 邮件发送服务
    /// </summary>
    public class EmailHelper
    {
        public IMessageService MessageService { get; set; }
        public ICommentService CommentService { get; set; }
        public IUserService UserService { get; set; }

        /// <summary>
        /// Email地址
        /// </summary>
        private static readonly string SendEmailAddress;
        /// <summary>
        /// 邮件服务SecretKey
        /// </summary>
        private static readonly string EmailSecretKey;
        /// <summary>
        /// 回访地址
        /// </summary>
        private static readonly string ReturnBackUrl;
        /// <summary>
        /// 站点地址
        /// </summary>
        private static readonly string SiteUrl;
        /// <summary>
        /// 站点名称
        /// </summary>
        private static readonly string SiteName;
        static EmailHelper()
        {
            SendEmailAddress = ConfigHelper.GetValue("EmailSendAddress");
            EmailSecretKey = ConfigHelper.GetValue("EmailSecretKey");
            ReturnBackUrl = ConfigHelper.GetValue("EmailReturnBackUrl");
            SiteUrl = ConfigHelper.GetValue("SiteUrl");
            SiteName = ConfigHelper.GetValue("SiteName");
        }

        /// <summary>
        /// 针对评论留言的发送邮件
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="sendEmailType"></param>
        /// <returns></returns>
        public bool ReplySendEmail(dynamic to, dynamic from, SendEmailType sendEmailType)
        {            
            var messageFrom = new MailAddress(SendEmailAddress); //发件人邮箱地址 
            UserDTO targetUser = UserService.GetUserByIdAsync(to.userId);
            UserDTO user = UserService.GetUserByIdAsync(from.userId);
            var messageTo = targetUser.email; //收件人邮箱地址 
            var messageSubject =  SiteName + "博客回复通知"; //邮件主题    
            string content;//目标的内容
            string replyContent;//回复的内容
            var url = ReturnBackUrl;//目标地址

            if (sendEmailType == SendEmailType.回复评论)
            {
                //评论回复
                var replyMessage = CommentService.GetCommentByIdAsync(from.id);
                replyContent = replyMessage.content;
                var message = CommentService.GetCommentByIdAsync(to.id);
                content = message.content;
                int articleId = message.articleId;
                url += "/details/" + articleId;
            }
            else
            {
                //留言回复
                var replyMessage = MessageService.GetMessageByIdAsync(from.id);
                replyContent = replyMessage.content;
                var message = MessageService.GetMessageByIdAsync(to.id);
                content = message.content;
                url += "/leacots";
            }
            //邮件内容
            var messageBody = "<div id=\"contentDiv\" onmouseover=\"getTop().stopPropagation(event);\" onclick=\"getTop().preSwapLink(event, 'spam', 'ZC3011-yZb5lAAS2SKCSSF8palnY9a');";
            messageBody += "style=\"position: relative; font - size:14px; height: auto; padding: 15px 15px 10px 15px; z - index:1; zoom: 1; line - height:1.7; \"";
            messageBody += "class=\"body\"><div id=\"qm_con_body\"><div id=\"mailContentContainer\" class=\"qmbox qm_con_body_content qqmail_webmail_only\" >";
            messageBody += "<div style=\"position: relative; color:#555;font:12px/1.5 Microsoft YaHei,Tahoma,Helvetica,Arial,sans-serif;max-width:600px;margin:50px auto;border-radius: 5px;box-shadow:0 5px 10px #aaaaaa;background: 0 0 repeat-x #FFF;background-image: -webkit-repeating-linear-gradient(135deg, #4882CE, #4882CE 20px, #FFF 20px, #FFF 35px, #EB1B2E 35px, #EB1B2E 55px, #FFF 55px, #FFF 70px);background-image: repeating-linear-gradient(-45deg, #4882CE, #4882CE 20px, #FFF 20px, #FFF 35px, #EB1B2E 35px, #EB1B2E 55px, #FFF 55px, #FFF 70px);background-size: 100% 10px\">";
            messageBody += "<div style=\"padding: 0 15px 8px;\"><h2 style=\"border-bottom:1px solid #e9e9e9;font-size:18px;font-weight:normal;padding:20px 0 10px;\"><span style=\"color: #12ADDB\">&gt;";
            messageBody += "</span>您在 <a style=\"text - decoration:none; color: #12ADDB;\" href="+SiteUrl+ " target =\"_blank\" rel=\"noopener\">" + SiteName + "博客</a> 中的评论或留言有新回复啦！</h2><div style=\"font-size:14px;color:#777;padding:0 10px;margin-top:18px\">";
            messageBody += "<p>" + targetUser.nickName + "，您曾在" + SiteName + "博客中发表评论或留言：</p><div style=\"background-color:#f5f5f5;padding:10px 15px;margin:18px 0;\">" + content + "</div><p>" + user.nickName + "回复说:</p><div style=\"background-color:#f5f5f5;padding: 10px 15px;margin:18px 0;\">" + replyContent + "</div>";
            messageBody += "<p>您可以点击<a style=\"text - decoration:none; color:#12addb\" href=" + url + " title=\"点击查看完整的回复内容\" target=\"_blank\" rel=\"noopener\">查看完整的回复內容</a>,欢迎回访<a style=\"text - decoration:none; color:#12addb\" href="+ SiteUrl + " target=\"_blank\" rel=\"noopener\">" + SiteName + "的博客</a>！</p>";
            messageBody += "</div></div><div style=\"color:#888;padding:10px;border-top:1px solid #e9e9e9;background:#f5f5f5;border-radius: 0 0 5px 5px;\">";
            messageBody += "<p style=\"margin: 0; padding: 0; \">Copyright ©<span style=\"border - bottom - width: 1px; border - bottom - style: dashed; border - bottom - color: rgb(204, 204, 204); z - index: 1; position: static; \"><span";
            messageBody += "style=\"border - bottom:1px dashed #ccc;z-index:1\" t=\"7\" onclick=\"return false;\" data=\"2019-2019\">2019-2020</span>";
            messageBody += "</span><a style=\"color:#888;text-decoration:none;\" href="+ SiteUrl + " target=\"_blank\" rel=\"noopener\">"+ SiteName + "博客</a>- 本邮件自动生成，请勿直接回复！</p>";
            messageBody += "</div></div><style type=\"text/css\">.qmbox style,.qmbox script,.qmbox head,.qmbox link,.qmbox meta {display: none !important;}</style></div></div>";
            messageBody += "<style>#mailContentContainer .txt {height: auto;}</style></div>";
            return SendMail(messageFrom, messageTo, messageSubject, messageBody);

        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="messageFrom"></param>
        /// <param name="messageTo"></param>
        /// <param name="messageSubject"></param>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        public static bool SendMail(MailAddress messageFrom, string messageTo, string messageSubject, string messageBody)               
        {
            var message = new MailMessage();
            message.To.Add(messageTo);
            message.From = messageFrom;
            message.Subject = messageSubject;
            message.SubjectEncoding = Encoding.UTF8;
            message.Body = messageBody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true; //是否为html格式 
            message.Priority = MailPriority.High; //发送邮件的优先等级 
            var sc = new SmtpClient
            {
                EnableSsl = true,
                Host = "smtp.qq.com",
                Port = 587,
                Credentials = new System.Net.NetworkCredential(SendEmailAddress, EmailSecretKey)
            };
            //是否SSL加密
            //指定发送邮件的服务器地址或IP 
            //指定发送邮件端口 

            sc.Send(message); //发送邮件 
            return true;
        }
    }
}
