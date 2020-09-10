using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;
using ZswBlog.MapperFactory;

namespace ZswBlog.Web.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly IArticleService articleService;
        private readonly IMessageService messageService;
        private readonly IUserService userService;
        private readonly ITagService tagService;
        private readonly IConfiguration conf;
        private readonly ArticleMapper articleMapper;
        private readonly MessageMapper messageMapper;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“IndexController.IndexController(IArticleService, IMessageService, IUserService, ITagService, IConfiguration, ArticleMapper, MessageMapper)”的 XML 注释
        public IndexController(IArticleService articleService, IMessageService messageService, IUserService userService, ITagService tagService, IConfiguration conf, ArticleMapper articleMapper, MessageMapper messageMapper)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“IndexController.IndexController(IArticleService, IMessageService, IUserService, ITagService, IConfiguration, ArticleMapper, MessageMapper)”的 XML 注释
        {
            this.articleService = articleService;
            this.messageService = messageService;
            this.userService = userService;
            this.tagService = tagService;
            this.conf = conf;
            this.articleMapper = articleMapper;
            this.messageMapper = messageMapper;
        }

        /// <summary>
        /// 获取初始化页面数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetInitData()
        {
            IndexInitDataDTO obj = await RedisHelper.GetAsync<IndexInitDataDTO>("ZswBlog:Index:InitData");
            if (obj != null)
            {
                return Ok(obj);
            }
            else
            {
                //首页的数据初始化数据
                List<Article> articles = await articleService.GetArticlesByPageAndIsShowAsync(3, 1, true);
                List<ArticleDTO> articleDTOs = await articleMapper.MapperToDTOsAsync(articles);

                List<Message> messages = (await messageService.GetMessagesOnNotReplyAsyncByPageAsync(10, 1)).ToList();
                List<MessageDTO> messageDTOs = new List<MessageDTO>();
                foreach (var item in messages)
                {
                    User user = await userService.GetUserByIdAsync(item.UserId);
                    MessageDTO messageDTO = await messageMapper.MapperToSingleDTOAsync(item, user);
                    messageDTOs.Add(messageDTO);
                }
                DateTime date1 = DateTime.Parse("2019-10-08 00:00:00");
                DateTime date2 = DateTime.Now;
                TimeSpan sp = date2.Subtract(date1);
                int tagCount = (await tagService.GetAllTagsAsync()).Count();
                int timeCount = sp.Days;
                int articleCount = (await articleService.GetAllArticlesAsync()).Count();
                int visitCount = await GetVisit();
                IndexInitDataDTO initDataDTO = new IndexInitDataDTO()
                {
                    DataCount = new CountData()
                    {
                        VisitsCount = visitCount,
                        TagsCount = tagCount,
                        RunDays = timeCount,
                        ArticleCount = articleCount
                    },
                    Articles = articleDTOs.ToList(),
                    Messages = messageDTOs
                };
                await RedisHelper.SetAsync("ZswBlog:Index:InitData", initDataDTO, 3600);
                return Ok(initDataDTO);
            }
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