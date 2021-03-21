using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.Query
{
    public class TravelSaveQuery
    {
        /// <summary>
        /// 旅行分享id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 旅行分享内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 是否显示1不显示,0显示
        /// </summary>
        public bool isShow { get; set; }

        /// <summary>
        /// 发布端
        /// </summary>
        public string createBy { get; set; }
        /// <summary>
        /// 附件列表
        /// </summary>
        public List<int> imgList { get; set; }
    }
}
