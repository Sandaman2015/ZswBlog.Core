
using Newtonsoft.Json;
using System;
using System.Text;
using System.Web;
using ZswBlog.Common.Util;

namespace ZswBlog.ThirdParty
{
    public class QQLogin : IDisposable
    {
        private IOAuthConfig config = new QQOAuthConfig(); //获取配置信息
        /// <summary>
        /// 获取Authorization Code请求地址，请求方法：Get
        /// </summary>
        /// <param name="callback">回调函数名称</param>
        /// <param name="state">client端的状态值。用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回。请务必严格按照流程检查用户与state参数状态的绑定。</param>
        /// <returns>返回Authorization Code请求地址</returns>
        public string GetAuthCodeUrl(string callback, out string state)
        {
            string api = "/oauth2.0/authorize";
            string callbackUrl = HttpUtility.UrlEncode(config.Domain + callback);//指向回调域名
            state = RandomHelper.GetRandomString(16);
            string url = string.Format("{0}{1}?response_type=code&client_id={2}&redirect_uri={3}&state={4}",
                config.BaseUrl, api, config.AppKey, callbackUrl, state);
            return url;
        }

        /// <summary>
        /// 获取Access Token请求地址，请求方法：Get
        /// </summary>
        /// <param name="code">Authorization Code</param>
        /// <param name="callback">回调函数名称</param>
        /// <returns>返回Access Token请求地址</returns>
        public string GetAccessTokenUrl(string code, string callback)
        {
            string api = "/oauth2.0/token";
            string callbackUrl = HttpUtility.UrlEncode(config.Domain + callback);
            string url = string.Format("{0}{1}?grant_type=authorization_code&client_id={2}&client_secret={3}&code={4}&redirect_uri={5}",
                config.BaseUrl, api, config.AppKey, config.AppSecret, code, callbackUrl);
            return url;
        }

        /// <summary>
        /// 获取Access Token
        /// </summary>
        /// <param name="url">Access Token请求地址</param>
        /// <returns>返回access_token</returns>
        public string GetAccessToken(string url)
        {
            try
            {
                string data = RequestHelper.HttpGet(url, Encoding.UTF8);
                //成功返回数据内容：access_token=FE04************************CCE2&expires_in=7776000&refresh_token=88E4************************BE14s
                //失败情况和其他需求，自行根据文档返回码完善
                string[] arr = data.Split('&');
                string[] temp = arr[0].Split('=');
                return temp[1];
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 获取用户OpenID
        /// </summary>
        /// <param name="access_token">获取到的access token</param>
        /// <returns>返回OpenID</returns>
        public string GetOpenID(string access_token)
        {
            try
            {
                string api = "/oauth2.0/me";
                string url = string.Format("{0}{1}?access_token={2}", config.BaseUrl, api, access_token);
                string data = RequestHelper.HttpGet(url, Encoding.UTF8);
                //返回数据内容：callback( { "client_id":"YOUR_APPID","openid":"YOUR_OPENID"} );
                int startIndex = data.IndexOf("(") + 1;
                int endIndex = data.IndexOf(")");
                int length = endIndex - startIndex;
                QQUser qqUser = JsonConvert.DeserializeObject<QQUser>(data.Substring(startIndex, length).Trim());
                return qqUser.Openid;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="access_token">获取到的access token</param>
        /// <param name="openid">用户的ID，与QQ号码一一对应。 </param>
        /// <returns>返回QQUserInfo</returns>
        public QQUserInfo GetQQUserInfo(string access_token, string openid)
        {
            try
            {
                string api = "/user/get_user_info";
                string url = string.Format("{0}{1}?access_token={2}&oauth_consumer_key={3}&openid={4}&format=json",
                    config.BaseUrl, api, access_token, config.AppKey, openid);
                string data = RequestHelper.HttpGet(url, Encoding.UTF8);
                return JsonConvert.DeserializeObject<QQUserInfo>(data);
            }
            catch
            {
                throw;
            }
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
