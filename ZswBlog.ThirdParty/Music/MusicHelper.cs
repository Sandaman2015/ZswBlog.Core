using System;
using System.Collections.Generic;
using System.Text;
using ZswBlog.Util;
using ZswBlog.DTO;
using Newtonsoft.Json;
using System.Linq;

namespace ZswBlog.ThirdParty.Music
{
    public class MusicHelper
    {

        private static string _baseMusicUrl;

        static MusicHelper()
        {
            _baseMusicUrl = ConfigHelper.GetValue("MusicBaseUrl");
        }

        public static List<MusicDTO> GetMusicListByCount(int count) {
            string url = ConfigHelper.GetValue("MusicBaseSite");
            string jsonResult = RequestHelper.HttpGet(_baseMusicUrl+url, Encoding.UTF8);
            MusicList musicList = JsonConvert.DeserializeObject<MusicList>(jsonResult);

            List<MusicDTO> musicDTOs = new List<MusicDTO>();
            List<MusicTracks> musicTracks= musicList.playlist.trackIds;
            //遍历歌单列表
            foreach (MusicTracks tracks in musicTracks)
            {
                count--;
                //获取歌曲详情
                var songsData =string.Format(_baseMusicUrl+"/song/detail?ids={0}",tracks.id);
                string dataResult = RequestHelper.HttpGet(songsData, Encoding.UTF8);
                MusicSongs musicSongs = JsonConvert.DeserializeObject<MusicSongs>(dataResult);

                //获取歌曲歌词
                var songsLyric = string.Format(_baseMusicUrl + "/lyric?id={0}", tracks.id);
                string lyricResult = RequestHelper.HttpGet(songsLyric, Encoding.UTF8);
                Musiclyric musiclyric = JsonConvert.DeserializeObject<Musiclyric>(lyricResult);

                //获取歌曲连接
                var songsUrl= string.Format(_baseMusicUrl + "/song/url?id={0}", tracks.id);
                string songsUrlResult = RequestHelper.HttpGet(songsUrl, Encoding.UTF8);
                MusicUrlData musicUrl = JsonConvert.DeserializeObject<MusicUrlData>(songsUrlResult);

                //填充歌曲歌词
                string lyric = null;

                //判断是有歌词和不为纯音乐
                if (!musiclyric.nolyric && !musiclyric.uncollected) {
                    lyric = musiclyric.lrc.lyric;
                }
                List<string> nameList= musicSongs.songs[0].ar.Select(a => a.name).ToList();                
                musicDTOs.Add(new MusicDTO()
                {
                    title = musicSongs.songs[0].name,
                    artist = string.Join(",", nameList),
                    pic=musicSongs.songs[0].al.picUrl,                    
                    lrc= lyric,
                    src= musicUrl.data[0].url
                });
                if (count == 0) {
                    break;
                }
            }
            return musicDTOs;
        }
    }
    
}
