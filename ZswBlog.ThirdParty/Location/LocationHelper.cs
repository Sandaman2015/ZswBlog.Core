using Newtonsoft.Json;
using System.Text;
using ZswBlog.Common.Util;

namespace ZswBlog.ThirdParty.Location
{
    /// <summary>
    /// 腾讯开发获取地址服务
    /// </summary>
    public static class LocationHelper
    {
        /// <summary>
        /// 位置服务
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetLocation(string ip)
        {
            var tencentApi = ConfigHelper.GetValue("TencentLocationApi");
            var locationKey = ConfigHelper.GetValue("LocationKey");
            var url = tencentApi + "?ip=" + ip + "&key=" + locationKey + "";
            var jsonResult = RequestHelper.HttpGet(url, Encoding.UTF8);
            var location = JsonConvert.DeserializeObject<LocationModel>(jsonResult);
            string address;
            if (location.result == null)
            {
                address = "中国";
            }
            else
            {
                var addInfo = location.result.ad_info;
                address = addInfo.province + addInfo.city + addInfo.district;
            }
            return address;
        }
    }
}
