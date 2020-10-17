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
            Dictionary<string, string> dic = new Dictionary<string, string>(4);
            dic.Add("BaseUrl", ConfigHelper.GetValue("QQBaseUrl"));
            dic.Add("AppKey", ConfigHelper.GetValue("QQAppKey"));
            dic.Add("AppSecret", ConfigHelper.GetValue("QQAppSecret"));
            dic.Add("Domain", ConfigHelper.GetValue("CallBackDomain"));
            return dic;
        }
    }

}
