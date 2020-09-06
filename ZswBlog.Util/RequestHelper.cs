using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ZswBlog.Util
{
    /// <summary>
    /// 简易HttpHelper类
    /// </summary>
    public class RequestHelper
    {
        public static string HttpGet(string url, Encoding encoding)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = "application/json;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 5000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, encoding);
            string json = myStreamReader.ReadToEnd();
            return json;

        }
        public static string HttpGet(string url, Dictionary<string, string> dic)
        {
            string param = GetParam(dic);
            string getUrl = string.Format("{0}?{1}", url, param);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(getUrl);
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            string result = "";
            using (StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        public static Stream HttpGet(string url)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.ContentType = "application/json;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 5000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response.GetResponseStream();
            //Stream myResponseStream = response.GetResponseStream();
            //StreamReader myStreamReader = new StreamReader(myResponseStream);
            //string json = myStreamReader.ReadToEnd();
            //return json;

        }


        public static string HttpPost(string url, Dictionary<string, string> dic)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            string param = GetParam(dic);
            byte[] data = Encoding.UTF8.GetBytes(param);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            string result = "";
            using (StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

       

        private static string GetParam(Dictionary<string, string> dic)
        {
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            return builder.ToString();
        }
    }

}
