using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;
using ZswBlog.MapperFactory;

namespace ZswBlog.Web.Controllers
{
    /// <summary>
    /// 标签页
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IArticleTagService articleTagService;
        private readonly ITagService tagService;
        private readonly TagMapper tagMapper;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“TagController.TagController(IArticleTagService, ITagService, TagMapper)”的 XML 注释
        public TagController(IArticleTagService articleTagService, ITagService tagService, TagMapper tagMapper)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“TagController.TagController(IArticleTagService, ITagService, TagMapper)”的 XML 注释
        {
            this.articleTagService = articleTagService;
            this.tagService = tagService;
            this.tagMapper = tagMapper;
        }

        /// <summary>
        /// 获取所有的文章标签
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleTagDTO>>> GetTagList()
        {
            IEnumerable<ArticleTagDTO> articleTagDTOs;
            //读取redis缓存
            List<ArticleTagDTO> articleTagsR = await RedisHelper.GetAsync<List<ArticleTagDTO>>("ZswBlog:Tasg:TagDTOList");
            if (articleTagsR != null)
            {
                articleTagDTOs = articleTagsR;
            }
            else
            {
                //开启缓存
                List<Tag> tags = await tagService.GetAllTagsAsync();//获取所有标签
                List<List<Article>> articles = await articleTagService.GetArticlesIdByTagIds(tags.Select(a => a.TagId).ToList());
                List<List<MiniArticleDTO>> dtos = await tagMapper.MapperToMiniDTOsAsync(articles);
                articleTagDTOs = await tagMapper.MapperToDTOsAsync(dtos, tags);
                await RedisHelper.SetAsync("ZswBlog:Tasg:TagDTOList", articleTagDTOs, 1200);
            }
            return Ok(articleTagDTOs);
        }
        /// <summary>
        /// 获取根据Id单个标签下的文章
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [HttpGet(template: "{tagId}")]
        public async Task<ActionResult<ArticleTagDTO>> GetArticleListByTagId(int tagId)
        {
            Tag tag = await tagService.GetTagByIdAsync(tagId);
            List<Article> articles = await articleTagService.GetArticlesIdByTagId(tag.TagId);
            List<MiniArticleDTO> miniArticleDTOs = await tagMapper.MapperToMiniDTOsAsync(articles);
            ArticleTagDTO articleTag = await tagMapper.SingleMapperToDTOsAsync(miniArticleDTOs, tag);
            return Ok(articleTag);
        }
    }
}
