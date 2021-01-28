using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using ZswBlog.Common.Constants;

namespace ZswBlog.Common.Util
{
    /// <summary>
    /// 简易HttpHelper类
    /// </summary>
    public static class RequestHelper
    {
        public static string HttpGet(string url, Encoding encoding)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = "application/json;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 50000;
            var response = (HttpWebResponse)request.GetResponse();
            var myResponseStream = response.GetResponseStream();
            var myStreamReader = new StreamReader(myResponseStream, encoding);
            var json = myStreamReader.ReadToEnd();
            return json;
        }
        public static string HttpGet(string url, Dictionary<string, string> dic)
        {
            var param = GetParam(dic);
            var getUrl = $"{url}?{param}";
            var req = (HttpWebRequest)WebRequest.Create(getUrl);
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            var resp = (HttpWebResponse)req.GetResponse();
            var result = "";
            using var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
            result = reader.ReadToEnd();
            return result;
        }

        public static Stream HttpGet(string url)
        {

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = "application/json;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 5000;
            var response = (HttpWebResponse)request.GetResponse();
            return response.GetResponseStream();
            //Stream myResponseStream = response.GetResponseStream();
            //StreamReader myStreamReader = new StreamReader(myResponseStream);
            //string json = myStreamReader.ReadToEnd();
            //return json;

        }


        public static string HttpPost(string url, Dictionary<string, string> dic)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            var param = GetParam(dic);
            var data = Encoding.UTF8.GetBytes(param);
            req.ContentLength = data.Length;
            using (var reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
            }
            var resp = (HttpWebResponse)req.GetResponse();
            var result = "";
            using (var reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

       

        private static string GetParam(Dictionary<string, string> dic)
        {
            var builder = new StringBuilder();
            var i = 0;
            foreach (var (key, value) in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", key, value);
                i++;
            }
            return builder.ToString();
        }
    }

}
