using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace Services
{
    public class FriendLinkService : BaseService, IFriendLinkService
    {
        public FriendLinkService(IFriendLinkRepository repository)
        {
            _repository = repository;
        }
        private IFriendLinkRepository _repository;
        public async Task<List<FriendLink>> GetAllFriendLinksAsync()
        {
            return await Task.Run(() =>
            {
                List<FriendLink> friendLinks = _repository.GetModels(a => a.FriendlinkId != 0).ToList();
                return friendLinks;
            });
        }

        public async Task<List<FriendLink>> GetFriendLinksByIsShowAsync(bool isShow)
        {
            return await Task.Run(() =>
            {
                List<FriendLink> friendLinks = _repository.GetModels(a => a.FriendlinkId != 0).ToList();
                return isShow ? friendLinks.Where(a => a.IsShow == 1).ToList() : friendLinks.Where(a => a.IsShow == 0).ToList();
            });
        }

        public async Task<bool> AddEntityAsync(FriendLink t)
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

        public Task<bool> AlterEntityAsync(FriendLink t)
        {
            throw new NotImplementedException();
        }
    }
}
