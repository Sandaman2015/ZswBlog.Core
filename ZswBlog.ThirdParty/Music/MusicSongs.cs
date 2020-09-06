using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.ThirdParty.Music
{
    /// <summary>
    /// 歌曲信息
    /// </summary>
    public class MusicSongs
    {
        public List<SongsList> songs { get; set; }
    }
    public class SongsList 
    {
        public string name { get; set; }
        public List<Ar> ar { get; set; }
        public Al al { get; set; }
    }
    /// <summary>
    /// 歌曲的歌手列表
    /// </summary>
    public class Ar
    {
        public long id { get; set; }
        public string name { get; set; }
        public List<string> tns { get; set; }
        public List<string> alias { get; set; }
    }

    /// <summary>
    /// 歌曲详情
    /// </summary>
    public class Al
    {
        /// <summary>
        /// 音乐id
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 音乐名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 音乐图片地址
        /// </summary>
        public string picUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> tns { get; set; }
        /// <summary>
        /// 图片字符串
        /// </summary>
        public string pic_str { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long pic { get; set; }
    }

}
