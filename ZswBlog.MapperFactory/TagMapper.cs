using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;

namespace ZswBlog.MapperFactory
{
    public class TagMapper
    {
        public TagMapper(IMapper mapper, IArticleTagService tagService)
        {
            _mapper = mapper;
        }
        private readonly IMapper _mapper;

        public async Task<List<ArticleTagDTO>> MapperToDTOsAsync(List<List<MiniArticleDTO>> articleDTOs, List<Tag> tags)
        {
            return await Task.Run(() =>
            {
                List<ArticleTagDTO> dtos = new List<ArticleTagDTO>();
                dtos = _mapper.Map<List<ArticleTagDTO>>(articleDTOs);

                for (int index = 0; index < dtos.ToList().Count(); index++)
                {
                    List<MiniArticleDTO> minis = new List<MiniArticleDTO>();
                    dtos.ToList()[index].TagName = tags.ToList()[index].TagName;
                }
                return dtos;
            });
        }

        public async Task<List<List<MiniArticleDTO>>> MapperToMiniDTOsAsync(List<List<Article>> articles)
        {
            return await Task.Run(() =>
            {
                return _mapper.Map<List<List<MiniArticleDTO>>>(articles);
            });
        }
        public async Task<List<MiniArticleDTO>> MapperToMiniDTOsAsync(List<Article> articles)
        {
            return await Task.Run(() =>
            {
                return _mapper.Map<List<MiniArticleDTO>>(articles);
            });
        }
        public async Task<ArticleTagDTO> SingleMapperToDTOsAsync(List<MiniArticleDTO> articles, Tag tag)
        {
            return await Task.Run(() =>
            {
                ArticleTagDTO ArticleTagDTO = _mapper.Map<ArticleTagDTO>(articles);
                ArticleTagDTO.TagName = tag.TagName;
                return ArticleTagDTO;
            });
        }
    }
}
