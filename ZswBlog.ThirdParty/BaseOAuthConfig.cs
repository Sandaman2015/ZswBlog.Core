using System.Collections.Generic;

namespace ZswBlog.ThirdParty
{
    internal abstract class BaseOAuthConfig : IOAuthConfig
    {
        private Dictionary<string, string> dicConfig = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseOAuthConfig()
        {
            dicConfig = GetConfig();
        }

        protected abstract Dictionary<string, string> GetConfig();

        #region 实现 IOAuthConfig 接口成员

        public string BaseUrl => dicConfig["BaseUrl"];

        public string AppKey => dicConfig["AppKey"];

        public string AppSecret => dicConfig["AppSecret"];

        public string Domain => dicConfig["Domain"];

        #endregion

    }


}
