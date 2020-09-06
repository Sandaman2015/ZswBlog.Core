namespace ZswBlog.ThirdParty
{
    public class LocationModel
    {

        /// <summary>
        /// 状态码
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public LocationResult result { get; set; }
    }
    public class Location
    {
        /// <summary>
        /// 
        /// </summary>
        public double lat { get; set; }

        /// <summary>
        /// 
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
        /// 
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
        /// 
        /// </summary>
        public string ip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Location location { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AddressInfo ad_info { get; set; }

    }

}
