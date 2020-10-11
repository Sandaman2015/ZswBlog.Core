//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using ZswBlog.DTO;
//using ZswBlog.Entity;
//using ZswBlog.IServices;
//using ZswBlog.MapperFactory;

//namespace ZswBlog.Web.Controllers
//{
//    /// <summary>
//    /// 文章页
//    /// </summary>
//    [Route("/api/[controller]/[action]")]
//    [ApiController]
//    public class WhisperController : ControllerBase
//    {
//        private readonly IUserService userService;
//        private readonly UserMapper userMapper;
//        private readonly IArticleService articleService;
//        private readonly ArticleMapper articleMapper;

//#pragma warning disable CS1591 // 缺少对公共可见类型或成员“WhisperController.WhisperController(IUserService, UserMapper, IArticleService, ArticleMapper)”的 XML 注释
//        public WhisperController(IUserService userService, UserMapper userMapper, IArticleService articleService, ArticleMapper articleMapper)
//#pragma warning restore CS1591 // 缺少对公共可见类型或成员“WhisperController.WhisperController(IUserService, UserMapper, IArticleService, ArticleMapper)”的 XML 注释
//        {
//            this.userService = userService;
//            this.userMapper = userMapper;
//            this.articleService = articleService;
//            this.articleMapper = articleMapper;
//        }

//        /// <summary>
//        /// 分页获取文章列表
//        /// </summary>
//        /// <param name="limit"></param>
//        /// <param name="pageIndex"></param>
//        /// <returns></returns>
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetArticlesByPage([FromQuery] int limit, [FromQuery] int pageIndex)
//        {
//            List<Article> articles = await articleService.GetArticlesByPageAndIsShow(limit, pageIndex, true);
//            List<ArticleDTO> articleDTOs = await articleMapper.MapperToDTOsAsync(articles);
//            return Ok(articleDTOs);
//        }

//        /// <summary>
//        /// 根据类型分页获取文章
//        /// </summary>
//        /// <param name="limit"></param>
//        /// <param name="pageIndex"></param>
//        /// <param name="classType"></param>
//        /// <returns></returns>
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetArticlesByClassPage([FromQuery] int limit, [FromQuery] int pageIndex, [FromQuery] string classType)
//        {
//            List<Article> articles = await articleService.GetArticlesByPageClassAndIsShowAsync(limit, pageIndex, classType, true);
//            List<ArticleDTO> articleDTOs = await articleMapper.MapperToDTOsAsync(articles);
//            return Ok(articleDTOs);
//        }
//        /// <summary>
//        /// 根据最喜爱的前五篇文章
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetArticlesOnTopLike()
//        {

//            IEnumerable<ArticleDTO> articleDTOs;
//            //读取缓存
//            List<ArticleDTO> articlesR = await RedisHelper.GetAsync<List<ArticleDTO>>("ZswBlog:Whisper:OnTop5Like");
//            if (articlesR != null)
//            {
//                articleDTOs = articlesR;
//            }
//            else
//            {
//                //开启redis缓存
//                List<Article> articles = await articleService.GetArticlesByTop5LikeAsync();
//                articleDTOs = await articleMapper.MapperToDTOsByLikeAsync(articles);
//                await RedisHelper.SetAsync("ZswBlog:Whisper:OnTop5Like", articleDTOs, 1200);
//            }
//            return Ok(articleDTOs);
//        }
//        /// <summary>
//        /// 获取浏览数最多的前五篇文章
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetArticlesOnTopVisit()
//        {
//            IEnumerable<ArticleDTO> articleDTOs;
//            //读取缓存
//            List<ArticleDTO> articlesR = await RedisHelper.GetAsync<List<ArticleDTO>>("ZswBlog:Whisper:OnTop5Visit");
//            if (articlesR != null)
//            {
//                articleDTOs = articlesR;
//            }
//            else
//            {
//                //开启redis缓存
//                List<Article> articles = await articleService.GetArticlesByTop5VisitAsync();
//                articleDTOs = await articleMapper.MapperToDTOsByVisitAsync(articles);
//                await RedisHelper.SetAsync("ZswBlog:Whisper:OnTop5Visit", articleDTOs, 1200);
//            }
//            return Ok(articleDTOs);
//        }

//        /// <summary>
//        /// 获取所有文章
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<string>>> GetArticlesOnClassType()
//        {
//            IEnumerable<string> classTypes = await articleService.GetArticlesByAllClassType();
//            return Ok(classTypes);
//        }

//        /// <summary>
//        /// 根据模糊查询获取文章
//        /// </summary>
//        /// <param name="title">biotic</param>
//        /// <returns></returns>
//        [HttpGet(template: "{title}")]
//        public async Task<ActionResult<IEnumerable<string>>> GetArticlesOnDimTitle(string title)
//        {
//            List<Article> articles = await articleService.GetArticlesByDimTitleAsync(title);
//            List<ArticleDTO> articleDTOs = await articleMapper.MapperToDTOsAsync(articles);
//            return Ok(articleDTOs);
//        }

//        /// <summary>
//        /// 获取最近登录用户
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUserOnNearLogin()
//        {
//            IEnumerable<UserDTO> userDTOs;
//            //读取缓存
//            List<UserDTO> userDTOsR = await RedisHelper.GetAsync<List<UserDTO>>("ZswBlog:Whisper:OnNearLogin");
//            if (userDTOsR != null)
//            {
//                userDTOs = userDTOsR;
//            }
//            else
//            {
//                //开启redis缓存
//                List<User> users = await userService.GetUsersNearVisit(12);
//                userDTOs = await userMapper.MapperToDTOsAsync(users);
//                await RedisHelper.SetAsync("ZswBlog:Whisper:OnNearLogin", userDTOs, 1200);
//            }
//            return Ok(userDTOs);
//        }
//    }
//}
