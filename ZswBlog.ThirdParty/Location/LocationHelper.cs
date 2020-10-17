using Newtonsoft.Json;
using System.Text;
using ZswBlog.Common.Util;

namespace ZswBlog.ThirdParty.Location
{
    public class LocationHelper
    {
        /// <summary>
        /// 位置服务
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetLocation(string ip)
        {
            string tenctApi = ConfigHelper.GetValue("TecentLocationApi");
            string locationKey = ConfigHelper.GetValue("LocationKey");
            string url = tenctApi + "?ip=" + ip + "&key=" + locationKey + "";
            string jsonResult = RequestHelper.HttpGet(url, Encoding.UTF8);
            LocationModel location = JsonConvert.DeserializeObject<LocationModel>(jsonResult);
            string address;
            if (location.result == null)
            {
                address = "中国";
            }
            else
            {
                AddressInfo addInfo = location.result.ad_info;
                address = addInfo.province + addInfo.city + addInfo.district;
            }
            return address;
        }
    }
}
