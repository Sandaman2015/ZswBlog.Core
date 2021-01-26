using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ZswBlog.Core.config
{
    /// <summary>
    /// ��־��¼����
    /// </summary>
    public class LogFilter: ActionFilterAttribute
    {
        private static readonly ILogger Logger = LoggerFactory.Create(build =>
        {
            build.AddConsole(); // ���ڿ���̨��������
            build.AddDebug(); // ����VS���ԣ�������ڵ����
        }).CreateLogger("LogFilter");
        

        /// <summary>
        /// ��־��¼
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var t = context.Controller.GetType();
            //��÷�����
            var actionName = context.RouteData.Values["action"]?.ToString();
            //�Ƿ��и�����
            var b = IsThatAttribute<FunctionDescriptionAttribute>(actionName, t);
            if (!b.DescriptionValue.Equals(string.Empty))
            {
                //��־��¼
                Logger.LogInformation(
                    $"������¼��{b.DescriptionValue}�� ����ʱ�䣺{DateTime.Now}, IP��ַ��{context.HttpContext.Connection.RemoteIpAddress}");    
            }
        }

        /// <summary>
        /// �ж��Ƿ���ӷ�������
        /// </summary>
        /// <param name="actionName">��������</param>
        /// <param name="t">��������</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static T IsThatAttribute<T>(string actionName, Type t) where T : new()
        {
            var attributes = t.GetMethod(actionName)?.GetCustomAttributes(typeof(T), true);
            if (attributes != null && attributes.Length>0)
            {
                return (T) attributes.GetValue(0);                
            }
            return new T();
        }
    }
}