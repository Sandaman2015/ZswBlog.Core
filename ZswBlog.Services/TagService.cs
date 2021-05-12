using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class TagService : BaseService<TagEntity, ITagRepository>, ITagService
    {
        public ITagRepository TagRepository { get; set; }
        public IMapper Mapper { get; set; }

        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        public async Task<List<TagDTO>> GetAllTagAsync()
        {
            return await Task.Run(() =>
            {
                var tags = TagRepository.GetModels(a => a.id != 0);
                return Mapper.Map<List<TagDTO>>(tags.ToList());
            });
        }

        /// <summary>
        /// 根据id获取标签
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public async Task<TagDTO> GetTagByIdAsync(int tagId)
        {
            var tag = await TagRepository.GetSingleModelAsync(a => a.id == tagId);
            return Mapper.Map<TagDTO>(tag);
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveEntityAsync(int tId)
        {
            var tag = await TagRepository.GetSingleModelAsync(a => a.id == tId);
            return await TagRepository.DeleteAsync(tag);
        }
    }
}