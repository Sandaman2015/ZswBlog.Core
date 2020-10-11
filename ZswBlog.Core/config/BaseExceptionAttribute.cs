using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ZswBlog.Core.config
{
    public class BaseExceptionAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;

            //处理各种异常
            context.ExceptionHandled = true;
            context.Result = new BaseExceptionResult((int)status, context.Exception);
        }
    }
}
