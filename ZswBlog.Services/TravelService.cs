using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace Services
{
    public class TravelService : BaseService, ITravelService
    {
        private ITravelRepository _repository { get; set; }

        public Task<bool> AddEntityAsync(Travel t)
        {
            return Task.Run(() => { return _repository.Add(t); });
        }

        public Task<bool> AlterEntityAsync(Travel t)
        {
            return Task.Run(() =>
            {
                return _repository.Update(t);
            });
        }

        public Task<List<Travel>> GetTravelsAsync()
        {
            return Task.Run(() =>
            {
                List<Travel> travels = _repository.GetModels(a => a.TravelId != 0).ToList();
                return travels;
            });
        }

        public Task<Travel> GetTravel(int tId)
        {
            return Task.Run(() =>
            {
                return _repository.GetSingleModel((Travel t) => t.TravelId == tId);
            });
        }

        public Task<bool> RemoveEntityAsync(int tId)
        {
            return Task.Run(() =>
            {
                Travel t = new Travel() { TravelId = tId };
                return _repository.Delete(t);
            });
        }
    }
}
