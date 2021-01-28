using System.Collections.Generic;
using ZswBlog.Common.Util;

namespace ZswBlog.ThirdParty
{
    internal sealed class QQOAuthConfig : BaseOAuthConfig
    {
        /// <summary>
        /// 获取QQ登录API配置
        /// </summary>
        /// <returns>返回配置信息</returns>
        protected override Dictionary<string, string> GetConfig()
        {
            var dic = new Dictionary<string, string>(4)
            {
                {"BaseUrl", ConfigHelper.GetValue("QQBaseUrl")},
                {"AppKey", ConfigHelper.GetValue("QQAppKey")},
                {"AppSecret", ConfigHelper.GetValue("QQAppSecret")},
                {"Domain", ConfigHelper.GetValue("CallBackDomain")}
            };
            return dic;
        }
    }

}
