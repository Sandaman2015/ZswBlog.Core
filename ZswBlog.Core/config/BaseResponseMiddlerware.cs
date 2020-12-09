using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZswBlog.Core.config
{
    /// <summary>
    /// 错误反馈中间件
    /// </summary>
    public class BaseResponseMiddlerware
    {
        private readonly RequestDelegate next;

        private static readonly ILogger _logger = LoggerFactory.Create(build =>
        {
            build.AddConsole();  // 用于控制台程序的输出
            build.AddDebug();    // 用于VS调试，输出窗口的输出
        }).CreateLogger("ExceptionResponseMiddlerware");
        public BaseResponseMiddlerware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var statusCode = context.Response.StatusCode;
                if (ex is ArgumentException)
                {
                    statusCode = 200;
                }
                await HandleExceptionAsync(context, statusCode, ex.Message);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                var msg = "";
                if (statusCode == 401)
                {
                    msg = "您未授权，请先获取授权";
                }
                else if (statusCode == 404)
                {
                    msg = "未找到服务";
                }
                else if (statusCode == 502)
                {
                    msg = "服务器处理请求错误";
                }
                else if (statusCode == 500)
                {
                    msg = "服务器内部错误";
                }
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionAsync(context, statusCode, msg);
                }
            }
        }
        //异常错误信息捕获，将错误信息用Json方式返回
        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
        {
            _logger.LogError("异常捕获输出：" + message + "     请求Ip:" + context.Connection.RemoteIpAddress.ToString());
            var result = JsonConvert.SerializeObject(new { success = false, msg = message, code = statusCode });
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }
    }
    //扩展方法
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BaseResponseMiddlerware>();
        }
    }
}
