using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class TimeLineService : BaseService<TimeLineEntity, ITimeLineRepository>, ITimeLineService
    {
        public ITimeLineRepository TimeLineRepository { get; set; }
        public IMapper Mapper { get; set; }

        /// <summary>
        /// 删除时间线
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public bool RemoveEntity(int tId)
        {
            TimeLineEntity timeLine = new TimeLineEntity()
            {
                id = tId,
            };
            return TimeLineRepository.Delete(timeLine);
        }

        /// <summary>
        /// 获取所有时间线列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<TimeLineDTO>> GetTimeLineListAsync()
        {
            var timeLines = TimeLineRepository.GetModels(a => a.id != 0);
            var list = await timeLines.OrderByDescending(a => a.createDate).ToListAsync();
            return Mapper.Map<List<TimeLineDTO>>(list);
        }

    }
}