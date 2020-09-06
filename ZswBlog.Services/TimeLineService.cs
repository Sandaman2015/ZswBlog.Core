using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace Services
{
    public class TimeLineService : BaseService, ITimeLineService
    {
        public TimeLineService(ITimeLineRepository repository)
        {
            _repository = repository;
        }
        private readonly ITimeLineRepository _repository;
        public Task<List<Timeline>> GetAllTimeLinesOrderByTimeAsync()
        {
            return Task.Run(() =>
            {
                List<Timeline> timeLines = _repository.GetModels(a => a.TimelineId != 0).OrderByDescending(a => a.TimelineCreateTime).ToList();
                return timeLines;
            });
        }

        public Task<bool> AddEntityAsync(Timeline t)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveEntityAsync(int tId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AlterEntityAsync(Timeline t)
        {
            throw new NotImplementedException();
        }
    }
}
