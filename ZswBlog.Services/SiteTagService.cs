using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace Services
{
    public class SiteTagService : BaseService, ISiteTagService
    {
        private ISiteTagRepository _repository;
        public SiteTagService(ISiteTagRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<SiteTag>> GetSiteTagsByIsShowAsync(bool isShow)
        {
            return await Task.Run(() =>
            {
                List<SiteTag> siteTags = _repository.GetModels(a => a.SitetagId != 0).ToList();
                return isShow ? siteTags.Where(a => a.IsShow == 1).ToList() : siteTags.Where(a => a.IsShow == 0).ToList();
            });
        }

        public async Task<List<SiteTag>> GetAllSiteTagsAsync()
        {
            return await Task.Run(() =>
            {
                List<SiteTag> siteTags = _repository.GetModels(a => a.SitetagId != 0).ToList();
                return siteTags;
            });
        }

        public async Task<bool> AddEntityAsync(SiteTag t)
        {
            return await Task.Run(() =>
            {
                return _repository.Add(t);
            });
        }

        public Task<bool> RemoveEntityAsync(int tId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AlterEntityAsync(SiteTag t)
        {
            throw new NotImplementedException();
        }
    }
}
