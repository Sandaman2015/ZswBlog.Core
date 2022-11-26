using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZswBlog.Common.Constants;

namespace ZswBlog.Common.Util
{
    /// <summary>
    /// 简易HttpHelper类
    /// </summary>
    public static class RequestHelper
    {

        static readonly HttpClient client = new HttpClient();

        public static async Task<string> HttpGet(string url, Encoding encoding)
        {
            var response = await client.GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            return json;
        }
    }

}
