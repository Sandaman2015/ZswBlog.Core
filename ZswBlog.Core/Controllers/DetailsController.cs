using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;
using ZswBlog.MapperFactory;
using ZswBlog.ThirdParty;
using ZswBlog.Util;

namespace ZswBlog.Web.Controllers
{
    /// <summary>
    /// 文章详情页
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class DetailsController : ControllerBase
    {
        private readonly IFileAttachmentService fileAttachmentService;
        private readonly IArticleService articleService;
        private readonly ITagService tagService;
        private readonly ICommentService commentService;
        private readonly IUserService userService;
        private readonly ArticleMapper articleMapper;
        private readonly CommentMapper commentMapper;
        private readonly EmailHelper emailHelper;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“DetailsController.DetailsController(IFileAttachmentService, IArticleService, ITagService, ICommentService, IUserService, ArticleMapper, CommentMapper, EmailHelper)”的 XML 注释
        public DetailsController(IFileAttachmentService fileAttachmentService, IArticleService articleService, ITagService tagService, ICommentService commentService, IUserService userService, ArticleMapper articleMapper, CommentMapper commentMapper, EmailHelper emailHelper)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“DetailsController.DetailsController(IFileAttachmentService, IArticleService, ITagService, ICommentService, IUserService, ArticleMapper, CommentMapper, EmailHelper)”的 XML 注释
        {
            this.fileAttachmentService = fileAttachmentService;
            this.articleService = articleService;
            this.tagService = tagService;
            this.commentService = commentService;
            this.userService = userService;
            this.articleMapper = articleMapper;
            this.commentMapper = commentMapper;
            this.emailHelper = emailHelper;
        }

        /// <summary>
        /// 根据文章id获取文章详情
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [HttpGet(template: "{index}")]
        public async Task<ActionResult<ArticleDTO>> GetArticle(int index)
        {
            ArticleDTO ArticleDTO;
            Article article = await articleService.GetArticleByIdAsync(index);//走索引所以不需要redis缓存      
            if (article != null)
            {
                Dictionary<string, string> dir = StringHelper.SplitArticleContent(article.ArticleContent);
                for (int i = 0; i < dir.Count; i++)
                {
                    string value = dir.ElementAt(i).Key;
                    dir[value] = (await fileAttachmentService.GetFilePathById(Guid.Parse(value)));
                }
                article.ArticleVisits++;//浏览数添加        
                await articleService.AlterEntityAsync(article);
                article.ArticleContent = StringHelper.ArticleContentReplace(article.ArticleContent, dir);
                ArticleDTO = await articleMapper.MapperToDTOAsync(article);
            }
            else
            {
                ArticleDTO = null;
            }
            return Ok(ArticleDTO);
        }

        /// <summary>
        /// 增加文章喜爱数
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet(template: "{articleId}")]
        public async Task<ActionResult> AddArticleLike(int articleId)
        {
            Article article = await articleService.GetArticleByIdAsync(articleId);//走索引所以不需要redis缓存
            article.ArticleLikes++;
            int Code = await articleService.AlterEntityAsync(article) ? 200 : 500;
            string str = Code == 200 ? "点赞成功！" : "点赞失败,刷新一下试试吧！";
            return Ok(new { msg = str, code = Code });
        }

        /// <summary>
        /// 获取所有文章标签列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetAllTags()
        {
            IEnumerable<Tag> tagsDTO;
            List<Tag> tagsR = await RedisHelper.GetAsync<List<Tag>>("ZswBlog:Details:TagDTOList");
            if (tagsR != null)
            {
                tagsDTO = tagsR;
            }
            else
            {
                IEnumerable<Tag> tags = await tagService.GetAllTagsAsync();
                tagsDTO = tags;
                await RedisHelper.SetAsync("ZswBlog:Details:TagDTOList", tags, 3600);
            }
            return Ok(tagsDTO);
        }
        /// <summary>
        /// 根据文章分页获取文章评论
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="limit"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentAggregationDTO>>> GetCommentsByPage(int articleId, int limit, int pageIndex)
        {
            List<Comment> comments = (await commentService.GetCommentsOnNotReplyAsyncByPageAsync(articleId, limit, pageIndex)).ToList();
            List<CommentAggregationDTO> commentsDTOs = new List<CommentAggregationDTO>();
            foreach (var item in comments)
            {
                CommentAggregationDTO commentAggregationDTO = new CommentAggregationDTO();//父元素
                User user = await userService.GetUserByIdAsync(item.UserId);
                CommentDTO commentDTO = await commentMapper.MapperToSingleDTOAsync(item, user);
                //递归查询子元素
                commentService.ClearRecursionComments();
                List<Comment> commentsChildren = commentService.GetCommentsByRecursion(item.CommentId);
                List<CommentDTO> commentChildrenDTOs = new List<CommentDTO>();//子集元素
                foreach (var that in commentsChildren)
                {
                    User userChildren = await userService.GetUserByIdAsync(that.UserId);
                    CommentDTO commentChildrenDTO = await commentMapper.MapperToSingleDTOAsync(that, userChildren);
                    commentChildrenDTO.TargetUserName = (userService.GetUserByIdAsync((Guid)that.TargetUserId)).Result.UserName;
                    commentChildrenDTOs.Add(commentChildrenDTO);
                }
                //汇总
                commentAggregationDTO.CommentChildren = commentChildrenDTOs.OrderBy(a => a.CommentDate).ToList();
                commentAggregationDTO.CommentParent = commentDTO;
                commentsDTOs.Add(commentAggregationDTO);
            }
            //获取总数
            int count = (await commentService.GetCommentsOnNotReplyAsync(articleId)).Count();
            return Ok(new { data = commentsDTOs, total = count });
        }
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddComment(Comment param)
        {
            int Code;
            string str;
            User user = await userService.GetUserByIdAsync(param.UserId);
            if (await commentService.IsExistsCommentOnNewestByUserId(param.UserId))
            {
                if (user.UserEmail != null && user.UserEmail != "")
                {
                    param.Location = await LocationHelper.GetLocation(param.Location);//获取位置
                    param.CommentDate = DateTime.Now;
                    Code = await commentService.AddEntityAsync(param) ? 200 : 500;
                    if (param.TargetId != 0 && param.TargetUserId != Guid.Empty && param.TargetUserId != null)
                    {
                        bool ok = await emailHelper.ReplySendEmail(await commentService.GetCommentByIdAsync(param.TargetId), await commentService.GetCommentByIdAsync(param.CommentId), SendEmailType.回复评论);//发送邮件
                        if (ok)
                        {
                            str = Code == 200 ? "回复成功！" : "回复失败！请刷新页面后重试！";
                        }
                        else
                        {
                            str = Code == 200 ? "回复成功,但是该回复并未通知到用户！" : "回复失败！请刷新页面后重试！";
                        }
                    }
                    else
                    {
                        str = Code == 200 ? "评论成功！" : "评论失败！请刷新页面后重试！";
                    }
                }
                else
                {
                    Code = 401;
                    str = "您还未填写邮箱，请填写邮箱后再评论，谢谢！";
                }
            }
            else
            {
                Code = 400;
                str = "您在1分钟已经提交过一次了，有什么想说的欢迎直接联系我哦！";
            }
            return Ok(new { code = Code, msg = str });
        }
    }
}
