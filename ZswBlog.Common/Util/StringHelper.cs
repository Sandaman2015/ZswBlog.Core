using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ZswBlog.Common.Util
{
    public class StringHelper
    {
        ///   <summary>   
        ///   将指定字符串按指定长度进行剪切，   
        ///   </summary>   
        ///   <param   name= "oldStr "> 需要截断的字符串 </param>   
        ///   <param   name= "maxLength "> 字符串的最大长度 </param>   
        ///   <param   name= "endWith "> 超过长度的后缀 </param>   
        ///   <returns> 如果超过长度，返回截断后的新字符串加上后缀，否则，返回原字符串 </returns>   
        public static string StringTruncat(string oldStr, int maxLength, string endWith, out int lastLength)
        {
            string pattern = @"[\u4e00-\u9fa5]";
            int i = 0;
            oldStr = System.Web.HttpUtility.HtmlEncode(oldStr);
            Regex rx = new Regex(pattern);
            if (rx.IsMatch(oldStr))
            {
                string newStr = "";
                MatchCollection mc = rx.Matches(oldStr);
                foreach (var str in mc)
                {
                    i++;
                    newStr += str.ToString();
                }
                lastLength = i > 0 ? i : 0;
                return newStr + endWith;
            }
            lastLength = i > 0 ? i : 0;
            return oldStr;
        }
        public static Dictionary<string, string> SplitArticleContent(string content)
        {
            string reg = "<img src=\"/_Framework(.+?)\"/>";
            Regex rx = new Regex(reg);
            Dictionary<string, string> files = new Dictionary<string, string>();
            if (rx.IsMatch(content))
            {
                MatchCollection mc = rx.Matches(content);
                foreach (var str in mc)
                {
                    string item = str.ToString()[10..^3].Substring(23, 36);
                    files.Add(item, null);
                }
            }
            return files;
        }

        public static string ArticleContentReplace(string content, Dictionary<string, string> dir)
        {
            string reg = "<img src=\"/_Framework(.+?)\"/>";
            Regex rx = new Regex(reg);
            if (rx.IsMatch(content))
            {
                MatchCollection mc = rx.Matches(content);
                foreach (var str in mc)
                {
                    string item = str.ToString().Substring(10, 93);
                    string value = item.ToString().Substring(23, 36);
                    content = content.Replace(item.ToString(), dir[value].Substring(17));
                }
            }
            return content;
        }

        /// <summary>
        /// 去除字符串中的Html标签只保留纯文本
        /// 去除以下标签闭合标签比如<img src="x.jpg" />
        /// 成对标签，只去除标签本身，不去除中间的内容 比如<pre>xx</pre>,将保留xx
        /// </summary>
        /// <param name="htmlContent">Html内容</param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ReplaceTag(string htmlContent, int length = 0)
        {
            string retValue =Regex.Replace(htmlContent, "<[^>]+>", "");
            retValue = Regex.Replace(retValue, "&[^;]+;", "");
            if (length > 0 && retValue.Length > length)
                return retValue.Substring(0, length);
            return retValue;
        }
    }
}
