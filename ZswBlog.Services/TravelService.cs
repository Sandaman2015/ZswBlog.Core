using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class TravelService : BaseService<TravelEntity, ITravelRepository>, ITravelService
    {
        private readonly ITravelRepository repository;

        public TravelService(ITravelRepository repository)
        {
            this.repository = repository;
        }

        public Task<bool> AddEntityAsync(Travel t)
        {
            return Task.Run(() => { return repository.Add(t); });
        }

        public Task<bool> AlterEntityAsync(Travel t)
        {
            return Task.Run(() =>
            {
                return repository.Update(t);
            });
        }

        public Task<List<Travel>> GetTravelsAsync()
        {
            return Task.Run(() =>
            {
                List<Travel> travels = repository.GetModels(a => a.TravelId != 0).ToList();
                return travels;
            });
        }

        public Task<Travel> GetTravel(int tId)
        {
            return Task.Run(() =>
            {
                return repository.GetSingleModel((Travel t) => t.TravelId == tId);
            });
        }

        public Task<bool> RemoveEntityAsync(int tId)
        {
            return Task.Run(() =>
            {
                Travel t = new Travel() { TravelId = tId };
                return repository.Delete(t);
            });
        }
    }
}
