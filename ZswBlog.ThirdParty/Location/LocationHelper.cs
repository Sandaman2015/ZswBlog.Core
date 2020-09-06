using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using ZswBlog.Util;

namespace ZswBlog.ThirdParty
{
    public class LocationHelper
    {
        /// <summary>
        /// 位置服务
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static async Task<string> GetLocation(string ip)
        {
            return await Task.Run(() =>
            {
                string tenctApi = ConfigHelper.GetValue("tecentLocationApi");
                string locationKey = ConfigHelper.GetValue("locationKey");
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
            });
        }
    }
}
