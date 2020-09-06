using System.IO;
using System.Net;
using System.Text;

namespace ZswBlog.Util
{
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
    }
}
