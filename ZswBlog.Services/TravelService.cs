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
            throw new NotImplementedException();
        }

        public Task<bool> AlterEntityAsync(Travel t)
        {
            throw new NotImplementedException();
        }

        public Task<List<Travel>> GetTravelsAsync()
        {
            return Task.Run(() =>
            {
                List<Travel> travels = _repository.GetModels(a => a.TravelId != 0).ToList();
                return travels;
            });
        }

        public Task<bool> RemoveEntityAsync(int tId)
        {
            throw new NotImplementedException();
        }
    }
}
