using System;
using System.Collections.Generic;
using System.Text;
using ZswBlog.DTO;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Common.Util;
using ZswBlog.Common.Exception;
using System.Text.Json.Nodes;
using ZswBlog.ThirdParty.Email;

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

        private static async Task<string> MusicEmailPasswordLogin()
        {
            string response = null;
            //登录
            try
            {
                var cloudMusicLogin = string.Format(BaseMusicUrl + "/login?email={0}&password={1}", MusicLoginName, MusicPassword);
                response = await RequestHelper.HttpGet(cloudMusicLogin, Encoding.UTF8);
                JsonObject keyValues = (JsonObject)JsonObject.Parse(response);
                var key = keyValues["code"].ToString();
                if (key != "200") {
                    await MusicQRLogin();
                }
            }
            catch (Exception ex)
            {
               await  MusicQRLogin();
            }
            return response;
        }


        private static async Task<string> MusicQRLogin()
        {
            string response = null;
            //切换二维码登录
            var qrKey = string.Format(BaseMusicUrl + "/login/qr/key");
            response = await RequestHelper.HttpGet(qrKey, Encoding.UTF8);
            JsonObject keyValues = (JsonObject)JsonObject.Parse(response);
            var key = keyValues["data"]["unikey"].ToString();
            var scanQr = string.Format(BaseMusicUrl + "/login/qr/create?key={0}&qrimg=1", key);
            response = await RequestHelper.HttpGet(scanQr, Encoding.UTF8);
            JsonObject qrImgValues = (JsonObject)JsonObject.Parse(response);
            var qrImg = qrImgValues["data"]["qrimg"].ToString();
            //发送二维码到邮箱
            bool sendOk = EmailHelper.SendMusicLoginQRCode(qrImg);
            if (sendOk)
            {
                //循环请求接口回调结果
                var flag = true;
                while (flag)
                {
                    var qrLoginStatus = string.Format(BaseMusicUrl + "/login/qr/check?key={0}", key);
                    response = await RequestHelper.HttpGet(qrLoginStatus, Encoding.UTF8);
                    JsonObject loginStatus = (JsonObject)JsonObject.Parse(response);
                    if (loginStatus["cookie"] != null)
                    {
                        response = loginStatus["cookie"].ToString();
                        await RedisHelper.SetAsync("music_cookie", response, 604800);
                        flag = false;
                        EmailHelper.MusicLoginConfirm();
                    }
                }
            }
            else
            {
                await MusicEmailPasswordLogin();
            }
            return response;
        }

        public static async Task<List<MusicDTO>> GetMusicListByCount(int count)
        {
            string cookie = null;
            string jsonResult = null;
            string authKey = null;
            try
            {
                cookie = await RedisHelper.GetAsync("music_cookie");
                authKey = "&cookie=" + cookie;
                var url = BaseMusicUrl + ConfigHelper.GetValue("MusicBaseSite") + authKey;
                jsonResult = await RequestHelper.HttpGet(url, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                cookie = await MusicEmailPasswordLogin();
                authKey = "&cookie=" + cookie;
                var url = BaseMusicUrl + ConfigHelper.GetValue("MusicBaseSite") + authKey;
                jsonResult = await RequestHelper.HttpGet(url, Encoding.UTF8);
            }
            var musicList = JsonConvert.DeserializeObject<MusicList>(jsonResult);
            if (musicList == null)
            {
                return new List<MusicDTO>();
            }
            var musicDtOs = new List<MusicDTO>();
            var musicTracks = musicList.playlist.trackIds;
            //遍历歌单列表
            foreach (var tracks in musicTracks)
            {
                count--;
                //获取歌曲详情
                var songsData = string.Format(BaseMusicUrl + "/song/detail?ids={0}", tracks.id) + authKey;
                var dataResult = await RequestHelper.HttpGet(songsData, Encoding.UTF8);
                var musicSongs = JsonConvert.DeserializeObject<MusicSongs>(dataResult);

                //获取歌曲歌词
                var songsLyric = string.Format(BaseMusicUrl + "/lyric?id={0}", tracks.id) + authKey;
                var lyricResult = await RequestHelper.HttpGet(songsLyric, Encoding.UTF8);
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
        }
    }
}