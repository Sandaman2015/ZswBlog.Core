using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZswBlog.Common.Exception
{
    public class BusinessException : System.Exception
    {
        public int code;
        public string msg;

        public BaseResultModel exModel;

        public BusinessException(string msg, int code)
        {
            exModel = new BaseResultModel()
            {
                Code = code,
                Message = msg,
                ReturnStatus = ReturnStatus.Error,
                Result = null
            };
        }

        public BusinessException(BaseResultModel baseResult)
        {
            exModel = baseResult;
        }
    }
}
