namespace ZswBlog.ThirdParty.Location
{
    public class LocationModel
    {

        /// <summary>
        /// 状态码
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 地址结果
        /// </summary>
        public LocationResult result { get; set; }
    }
    public class Location
    {
        /// <summary>
        /// 经度
        /// </summary>
        public double lat { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double lng { get; set; }

    }



    public class AddressInfo
    {
        /// <summary>
        /// 中国
        /// </summary>
        public string nation { get; set; }

        /// <summary>
        /// 安徽省
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 芜湖市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 街道
        /// </summary>
        public string district { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int adcode { get; set; }

    }



    public class LocationResult
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string ip { get; set; }

        /// <summary>
        /// 位置信息
        /// </summary>
        public Location location { get; set; }

        /// <summary>
        /// 地址信息
        /// </summary>
        public AddressInfo ad_info { get; set; }

    }

}
