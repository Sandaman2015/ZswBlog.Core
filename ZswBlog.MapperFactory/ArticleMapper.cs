using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;
using ZswBlog.Util;

namespace ZswBlog.MapperFactory
{
    public class ArticleMapper
    {
        public ArticleMapper(IMapper mapper, IArticleTagService tagService, IFileAttachmentService fileAttachmentService)
        {
            _mapper = mapper;
            _service = tagService;
            _fileService = fileAttachmentService;
        }
        private readonly IMapper _mapper;
        private readonly IArticleTagService _service;
        private readonly IFileAttachmentService _fileService;

        public async Task<List<ArticleDTO>> MapperToDTOsAsync(List<ArticleEntity> articles)
        {
            List<ArticleDTO> ArticleDTO = _mapper.Map<List<ArticleDTO>>(articles);
            foreach (var item in ArticleDTO)
            {
                List<Tag> tags = await _service.GetTagsIdByArticleId(item.ArticleId);
                item.ArticleTags = tags.Select(a => a.TagName).ToArray();
                item.ArticleCreatedBy = "Sandman";
                item.ArticleContent = StringHelper.StringTruncat(item.ArticleContent, 200, "\r\n....", out int lastLength);
                item.ArticleTextCount = lastLength;
                item.ArticleReadTime = lastLength / 250;
                item.ArticleImage = (await _fileService.GetFilePathById(Guid.Parse(item.ArticleImage))).Substring(17);
            }
            return ArticleDTO;
        }
        public async Task<ArticleDTO> MapperToDTOAsync(Article articles)
        {
            ArticleDTO ArticleDTO = _mapper.Map<ArticleDTO>(articles);
            ArticleDTO.ArticleCreatedBy = "Sandman";
            List<Tag> tags = await _service.GetTagsIdByArticleId(articles.ArticleId);
            ArticleDTO.ArticleTags = tags.Select(a => a.TagName).ToArray();
            StringHelper.StringTruncat(ArticleDTO.ArticleContent, 200, "\r\n....", out int lastLength);
            ArticleDTO.ArticleTextCount = lastLength;
            ArticleDTO.ArticleReadTime = lastLength / 250;
            ArticleDTO.ArticleImage = (await _fileService.GetFilePathById(Guid.Parse(ArticleDTO.ArticleImage))).Substring(17);
            return ArticleDTO;
        }

        public async Task<List<ArticleDTO>> MapperToDTOsByLikeAsync(List<ArticleEntity> articles)
        {
            return await Task.Run(() =>
            {
                List<ArticleDTO> ArticleDTO = _mapper.Map<List<ArticleDTO>>(articles);
                return ArticleDTO;
            });
        }

        public async Task<List<ArticleDTO>> MapperToDTOsByVisitAsync(List<ArticleEntity> articles)
        {
            return await Task.Run(() =>
            {
                List<ArticleDTO> ArticleDTO = _mapper.Map<List<ArticleDTO>>(articles);
                return ArticleDTO;
            });
        }
    }
}
