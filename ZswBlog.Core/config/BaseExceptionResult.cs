using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using ZswBlog.Common;

namespace ZswBlog.Core.config
{
    public class BaseExceptionResult : ObjectResult
    {
        public BaseExceptionResult(int? code, Exception exception)
           : base(new BaseExceptionResultModel(code, exception))
        {
            StatusCode = code;
        }
    }
}
