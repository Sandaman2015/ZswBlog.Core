using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZswBlog.Common
{
    public class BaseExceptionResultModel : BaseResultModel
    {
        public BaseExceptionResultModel(int? code, Exception exception)
        {
            Code = code;
            Message = exception.InnerException != null ?
                exception.InnerException.Message :
                exception.Message;
            Result = exception.Message;
            ReturnStatus = ReturnStatus.Error;
        }
    }
}
