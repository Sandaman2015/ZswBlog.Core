using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.ThirdParty.Music
{
    public class MusicList
    {
        public long code { get; set; }
        public PlayList playlist { get; set; }
    }

    /// <summary>
    /// 歌单列表
    /// </summary>
    public class MusicTracks
    {
        /// <summary>
        /// 音乐id
        /// </summary>
        public long id { get; set; }
        public long v { get; set; }
        public long at { get; set; }
        public string alg { get; set; }
    }
   
    public class PlayList 
    {
        public List<MusicTracks> trackIds { get; set; }
    }
}
