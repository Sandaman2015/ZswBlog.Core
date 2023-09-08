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
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.Method = "GET";
            //request.ContentType = "text/html;charset=UTF-8";
            //request.UserAgent = null;

            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //Stream myResponseStream = response.GetResponseStream();
            //StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            //string retString = myStreamReader.ReadToEnd();
            //myStreamReader.Close();
            //myResponseStream.Close();

            //return retString;
        }
    }

}
