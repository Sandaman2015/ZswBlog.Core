using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class TagService : BaseService<TagEntity, ITagRepository>, ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagService(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        public List<TagDTO> GetAllTag()
        {
            List<TagEntity> tags = _tagRepository.GetModels(a => a.id != 0).ToList();
            return _mapper.Map<List<TagDTO>>(tags);
        }

        /// <summary>
        /// 根据id获取标签
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public TagDTO GetTagById(int tagId)
        {
            TagEntity tag = _tagRepository.GetSingleModel(a => a.id == tagId);
            return _mapper.Map<TagDTO>(tag);
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public bool RemoveEntity(int tId)
        {
            TagEntity tag = _tagRepository.GetSingleModel(a => a.id == tId);
            return _tagRepository.Delete(tag);
        }
    }
}
