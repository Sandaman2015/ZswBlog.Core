using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZswBlog.DTO;
using ZswBlog.ThirdParty.Music;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 通用控制器
    /// </summary>
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        /// <summary>
        /// 获取歌曲列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<MusicDTO>>> GetMusicList()
        {
            List<MusicDTO> musicDTOs = await RedisHelper.GetAsync<List<MusicDTO>>("ZswBlog:Common:MusicList");
            if (musicDTOs == null)
            {
                musicDTOs = MusicHelper.GetMusicListByCount(30);
                RedisHelper.SetAsync("ZswBlog:Common:MusicList", musicDTOs, 60 * 60 * 12);
            }
            return Ok(musicDTOs);
        }

        /// <summary>
        /// 获取所有歌曲列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<MusicDTO>>> GetAllMusicList()
        {
            List<MusicDTO> musicDTOs = await RedisHelper.GetAsync<List<MusicDTO>>("ZswBlog:Common:MusicAllList");
            if (musicDTOs == null)
            {
                musicDTOs = MusicHelper.GetMusicListByCount(100);
                RedisHelper.SetAsync("ZswBlog:Common:MusicAllList", musicDTOs, 60 * 60 * 12);
            }
            return Ok(musicDTOs);
        }
    }
}
