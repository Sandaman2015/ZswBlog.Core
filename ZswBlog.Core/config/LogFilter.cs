using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ZswBlog.Core.config
{
    /// <summary>
    /// 日志记录操作
    /// </summary>
    public class LogFilter: ActionFilterAttribute
    {
        private static readonly ILogger Logger = LoggerFactory.Create(build =>
        {
            build.AddConsole(); // 用于控制台程序的输出
            build.AddDebug(); // 用于VS调试，输出窗口的输出
        }).CreateLogger("LogFilter");
        

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var t = context.Controller.GetType();
            //获得方法名
            var actionName = context.RouteData.Values["action"]?.ToString();
            //是否有该特性
            var b = IsThatAttribute<FunctionDescriptionAttribute>(actionName, t);
            if (!b.DescriptionValue.Equals(string.Empty))
            {
                //日志记录
                Logger.LogInformation(
                    $"操作记录：{b.DescriptionValue}， 操作时间：{DateTime.Now}, IP地址：{context.HttpContext.Connection.RemoteIpAddress}");    
            }
        }

        /// <summary>
        /// 判断是否添加方法特性
        /// </summary>
        /// <param name="actionName">方法名称</param>
        /// <param name="t">特性类型</param>
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