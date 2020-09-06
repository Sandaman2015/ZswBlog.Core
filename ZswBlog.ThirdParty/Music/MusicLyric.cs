using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.ThirdParty.Music
{
    /// <summary>
    /// 歌词对象
    /// </summary>
    public class Musiclyric
    {
        /// <summary>
        /// 没有歌词
        /// </summary>
        public bool uncollected { get; set; }
        /// <summary>
        /// 纯音乐代表
        /// </summary>
        public bool nolyric { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool sgc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool sfy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool qfy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Lrc lrc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
    }

    public class Lrc
    {
        /// <summary>
        /// 
        /// </summary>
        public int version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lyric { get; set; }
    }

}
