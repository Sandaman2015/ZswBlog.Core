using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.DTO
{
    /// <summary>
    /// 音乐返回DTO
    /// </summary>
    public class MusicDTO
    {
        /// <summary>
        /// 音乐标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 演唱人
        /// </summary>
        public string artist { get; set; }
        /// <summary>
        /// 音乐地址
        /// </summary>
        public string src { get; set; }
        /// <summary>
        /// 音乐logo
        /// </summary>
        public string pic { get; set; }
        /// <summary>
        /// 音乐歌词
        /// </summary>
        public string lrc { get; set; }
    }
}
