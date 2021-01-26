using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ZswBlog.Core.config
{
    /// <summary>
    /// 错误反馈中间件
    /// </summary>
    public class BaseResponseMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly ILogger Logger = LoggerFactory.Create(build =>
        {
            build.AddConsole(); // 用于控制台程序的输出
            build.AddDebug(); // 用于VS调试，输出窗口的输出
        }).CreateLogger("ExceptionResponseMiddleWare");

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="next"></param>
        public BaseResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
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
                var msg = statusCode switch
                {
                    401 => "您未授权，请先获取授权",
                    404 => "未找到服务",
                    502 => "服务器处理请求错误",
                    500 => "服务器内部错误",
                    _ => ""
                };
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionAsync(context, statusCode, msg);
                }
            }
        }

        /// <summary>
        /// 异常返回执行方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        //异常错误信息捕获，将错误信息用Json方式返回
        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
        {
            if (context.Connection.RemoteIpAddress != null)
                Logger.LogError(
                    $"请求路径：{context.Request.Path.Value}, 异常捕获输出：{message}, IP地址：{context.Connection.RemoteIpAddress}");
            var result = JsonConvert.SerializeObject(new {success = false, msg = message, code = statusCode});
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }
    }

    /// <summary>
    /// 异常处理继承
    /// </summary>
    public static class ErrorHandlingExtensions
    {
        /// <summary>
        /// 扩展方法
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BaseResponseMiddleware>();
        }
    }
}