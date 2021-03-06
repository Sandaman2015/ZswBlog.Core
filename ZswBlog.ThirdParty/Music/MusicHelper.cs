﻿using System;
using System.Collections.Generic;
using System.Text;
using ZswBlog.DTO;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Common.Util;
using ZswBlog.Common.Exception;

namespace ZswBlog.ThirdParty.Music
{
    /// <summary>
    /// 获取音乐
    /// </summary>
    public static class MusicHelper
    {
        private static readonly string BaseMusicUrl;
        private static readonly string SongMusicUrl;
        private static readonly string MusicLoginName;
        private static readonly string MusicPassword;

        static MusicHelper()
        {
            BaseMusicUrl = ConfigHelper.GetValue("MusicBaseUrl");
            SongMusicUrl = ConfigHelper.GetValue("SongBaseUrl");
            MusicLoginName = ConfigHelper.GetValue("MusicLoginName");
            MusicPassword = ConfigHelper.GetValue("MusicPassword");
        }

        public static async Task<List<MusicDTO>> GetMusicListByCount(int count)
        {
            return await Task.Run(() =>
            {
                //登录详情
                var cloudMusicLogin = string.Format(BaseMusicUrl + "/login?email={0}&password={1}", MusicLoginName, MusicPassword);
                var loginDetail = RequestHelper.HttpGet(cloudMusicLogin, Encoding.UTF8);
                MusicLoginDetails details = JsonConvert.DeserializeObject<MusicLoginDetails>(loginDetail);
                if (details == null) {
                    throw new BusinessException("网易云登录请求失败", 401);
                }
                var authKey = "&cookie=" + details.cookie;
                var url = BaseMusicUrl + ConfigHelper.GetValue("MusicBaseSite") + authKey;
                var jsonResult = RequestHelper.HttpGet(url, Encoding.UTF8);
                var musicList = JsonConvert.DeserializeObject<MusicList>(jsonResult);
                var musicDtOs = new List<MusicDTO>();
                var musicTracks = musicList.playlist.trackIds;
                //遍历歌单列表
                foreach (var tracks in musicTracks)
                {
                    count--;
                    //获取歌曲详情
                    var songsData = string.Format(BaseMusicUrl + "/song/detail?ids={0}", tracks.id) + authKey;
                    var dataResult = RequestHelper.HttpGet(songsData, Encoding.UTF8);
                    var musicSongs = JsonConvert.DeserializeObject<MusicSongs>(dataResult);

                    //获取歌曲歌词
                    var songsLyric = string.Format(BaseMusicUrl + "/lyric?id={0}", tracks.id) + authKey;
                    var lyricResult = RequestHelper.HttpGet(songsLyric, Encoding.UTF8);
                    var musicLyric = JsonConvert.DeserializeObject<Musiclyric>(lyricResult);

                    ////获取歌曲连接
                    //var songsUrl= string.Format(_baseMusicUrl + "/song/url?id={0}", tracks.id);
                    //string songsUrlResult = RequestHelper.HttpGet(songsUrl, Encoding.UTF8);
                    //MusicUrlData musicUrl = JsonConvert.DeserializeObject<MusicUrlData>(songsUrlResult);

                    //填充歌曲歌词
                    string lyric = null;

                    //判断是有歌词和不为纯音乐
                    if (!musicLyric.nolyric && !musicLyric.uncollected)
                    {
                        lyric = musicLyric.lrc.lyric;
                    }
                    var nameList = musicSongs.songs[0].ar.Select(a => a.name).ToList();
                    musicDtOs.Add(new MusicDTO()
                    {
                        name = musicSongs.songs[0].name,
                        artist = string.Join(",", nameList),
                        cover = musicSongs.songs[0].al.picUrl,
                        lrc = lyric,
                        url = SongMusicUrl + tracks.id + ".mp3"
                    });
                    if (count == 0)
                    {
                        break;
                    }
                }
                return musicDtOs;
            });
        }
    }
}