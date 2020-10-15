using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ZswBlog.Core.config
{
    public class BaseExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// 触发异常捕获
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;

            //处理各种异常
            context.ExceptionHandled = true;
            context.Result = new BaseExceptionResult((int)status, context.Exception);
        }
    }
}
