using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ZswBlog.DTO;
using ZswBlog.IServices;
using ZswBlog.ThirdParty.Music;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 通用控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IMessageService _messageService;
        private readonly ITagService _tagService;
        private readonly IConfiguration conf;

        public CommonController(IMessageService messageService, IArticleService articleService, ITagService tagService, IConfiguration conf)
        {
            _messageService = messageService;
            _articleService = articleService;
            _tagService = tagService;
            this.conf = conf;
        }

        /// <summary>
        /// 获取歌曲列表
        /// </summary>
        /// <returns></returns>
        [Route("/music/get/top")]
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
        [Route("/music/get/all")]
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

        /// <summary>
        /// 获取初始化页面数据
        /// </summary>
        /// <returns></returns>
        [Route("/common/get/init")]
        [HttpGet]
        public async Task<ActionResult> GetInitData()
        {
            IndexInitDataDTO initDataDTO;
            //首页的数据初始化数据
            initDataDTO = await RedisHelper.GetAsync<IndexInitDataDTO>("ZswBlog:Common:InitData");
            if (initDataDTO == null)
            {
                List<ArticleDTO> articles = _articleService.GetArticlesByPageAndIsShow(3, 1, true).data;
                List<MessageDTO> messages = _messageService.GetMessageOnNoReplyAndCount(10);
                DateTime date1 = DateTime.Parse("2019-10-08 00:00:00");
                DateTime date2 = DateTime.Now;
                TimeSpan sp = date2.Subtract(date1);
                int tagCount = _tagService.GetEntitiesCount();
                int timeCount = sp.Days;
                int articleCount = _articleService.GetEntitiesCount();
                int visitCount = await GetVisit();
                initDataDTO = new IndexInitDataDTO()
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
                await RedisHelper.SetAsync("ZswBlog:Common:InitData", initDataDTO, 60 * 60 * 3);
            }
            return Ok(initDataDTO);
        }

        private async Task<int> GetVisit()
        {
            string filepath = Path.Combine(Directory.GetCurrentDirectory(), conf.GetValue<string>("filePath"));
            FileStream fileStream = new FileStream(filepath, FileMode.Open);
            StreamReader reader;
            StreamWriter writer;
            int visit = 0;
            try
            {
                reader = new StreamReader(fileStream);
                string line = await reader.ReadLineAsync();
                visit = int.Parse(line);
                fileStream.Flush();
                reader.Close();
                writer = new StreamWriter(filepath, append: false);
                await writer.WriteLineAsync((visit + 1).ToString());
                await writer.DisposeAsync();
            }
            catch (IOException e)
            {
                throw e;
            }
            finally
            {
                fileStream.Close();
            }
            return visit;
        }
    }
}
