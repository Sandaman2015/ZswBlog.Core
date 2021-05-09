using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using ZswBlog.Common;
using ZswBlog.Common.Util;
using ZswBlog.Core.config;
using ZswBlog.DTO;
using ZswBlog.IServices;
using ZswBlog.ThirdParty.Music;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 通用控制器
    /// </summary>
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IMessageService _messageService;
        private readonly ISiteTagService _tagService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="messageService"></param>
        /// <param name="articleService"></param>
        /// <param name="tagService"></param>
        /// <param name="configuration"></param>
        public CommonController(IMessageService messageService, IArticleService articleService,
            ISiteTagService tagService, IConfiguration configuration)
        {
            _messageService = messageService;
            _articleService = articleService;
            _tagService = tagService;
            _configuration = configuration;
        }

        /// <summary>
        /// 获取歌曲列表
        /// </summary>
        /// <returns></returns>
        [Route("/api/music/get/top")]
        [HttpGet]
        [FunctionDescription("获取歌曲列表")]
        public async Task<ActionResult<List<MusicDTO>>> GetMusicList()
        {
            var musicDtOs = await RedisHelper.GetAsync<List<MusicDTO>>("ZswBlog:Common:MusicList");
            if (musicDtOs != null) return Ok(musicDtOs);
            musicDtOs = await MusicHelper.GetMusicListByCount(30);
            await RedisHelper.SetAsync("ZswBlog:Common:MusicList", musicDtOs, 2400);
            return Ok(musicDtOs);
        }

        /// <summary>
        /// 获取所有歌曲列表
        /// </summary>
        /// <returns></returns>
        [Route("/api/music/get/all")]
        [HttpGet]
        [FunctionDescription("获取所有歌曲列表")]
        public async Task<ActionResult<List<MusicDTO>>> GetAllMusicList()
        {
            var musicDtOs = await RedisHelper.GetAsync<List<MusicDTO>>("ZswBlog:Common:MusicList");
            if (musicDtOs != null) return Ok(musicDtOs);
            musicDtOs = await MusicHelper.GetMusicListByCount(50);
            await RedisHelper.SetAsync("ZswBlog:Common:MusicList", musicDtOs, 2400);
            return Ok(musicDtOs);
        }

        /// <summary>
        /// 获取初始化页面数据
        /// </summary>
        /// <returns></returns>
        [Route("/api/common/get/init")]
        [HttpGet]
        [FunctionDescription("获取首页初始化数据")]
        public async Task<ActionResult> GetInitData()
        {
            //首页的数据初始化数据
            var initDataDto = await RedisHelper.GetAsync<IndexInitDataDTO>("ZswBlog:Common:InitData");
            if (initDataDto != null) return Ok(initDataDto);
            var articles = await _articleService.GetArticlesByNearSaveAsync(3);
            var messages = await _messageService.GetMessageOnNearSaveAsync(10);
            var date1 = DateTime.Parse("2019-10-08 00:00:00");
            var date2 = DateTime.Now;
            var sp = date2.Subtract(date1);
            var tagCount = await _tagService.GetAllSiteTagsCountAsync();
            var timeCount = sp.Days;
            var articleCount = await _articleService.GetEntitiesCountAsync();
            var visitCount = await GetVisit();
            initDataDto = new IndexInitDataDTO()
            {
                DataCount = new CountData()
                {
                    VisitsCount = visitCount,
                    TagsCount = tagCount,
                    RunDays = timeCount,
                    ArticleCount = articleCount
                },
                Articles = articles,
                Messages = messages
            };
            await RedisHelper.SetAsync("ZswBlog:Common:InitData", initDataDto, 60 * 60 * 3);

            return Ok(initDataDto);
        }

        /// <summary>
        /// 获取浏览数
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetVisit()
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), _configuration.GetValue<string>("filePath"));
            var fileStream = new FileStream(filepath, FileMode.Open);
            int visit;
            try
            {
                var reader = new StreamReader(fileStream);
                var line = await reader.ReadLineAsync();
                visit = int.Parse(line ?? string.Empty);
                fileStream.Flush();
                reader.Close();
                var writer = new StreamWriter(filepath, append: false);
                await writer.WriteLineAsync((visit + 1).ToString());
                await writer.DisposeAsync();
            }
            finally
            {
                fileStream.Close();
            }

            return visit;
        }
    }
}