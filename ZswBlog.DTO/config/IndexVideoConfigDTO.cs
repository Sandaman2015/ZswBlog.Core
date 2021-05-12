using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZswBlog.DTO.config
{
    /// <summary>
    /// 首页配置对象
    /// </summary>
    public class IndexVideoConfigDTO
    {
        /// <summary>
        /// 视频路径
        /// </summary>
        public string videosrc { get; set; }
        /// <summary>
        /// 视频简照
        /// </summary>
        public string poster { get; set; }
        /// <summary>
        /// 初始音量
        /// </summary>
        public float volume { get; set; }
    }
}
