using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ZswBlog.Common;

namespace ZswBlog.Core.config
{
    /// <summary>
    /// API接口返回类型
    /// </summary>
    public class ApiResultFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 结果返回
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var objectResult = context.Result as ObjectResult;
            if (objectResult != null)
                context.Result =
                    new OkObjectResult(new BaseResultModel(code: objectResult.StatusCode, result: objectResult.Value));
        }
    }
}