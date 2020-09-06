using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace Services
{
    public class TagService : BaseService, ITagService
    {
        public TagService(ITagRepository repository)
        {
            _repository = repository;
        }
        private ITagRepository _repository;
        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await Task.Run(() =>
             {
                 List<Tag> tags = _repository.GetModels(a => a.TagId != 0).ToList();
                 return tags;
             });
        }

        public async Task<Tag> GetTagByIdAsync(int tagId)
        {
            return await Task.Run(() =>
            {
                Tag tags = _repository.GetSingleModel(a => a.TagId == tagId);
                return tags;
            });
        }

        public Task<bool> AddEntityAsync(Tag t)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveEntityAsync(int tId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AlterEntityAsync(Tag t)
        {
            throw new NotImplementedException();
        }
    }
}
