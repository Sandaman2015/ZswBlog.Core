using AutoMapper;
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
        public async Task<bool> RemoveEntityAsync(int tId)
        {
            var timeLine = await TimeLineRepository.GetSingleModelAsync(a => a.id == tId);
            return await TimeLineRepository.DeleteAsync(timeLine);
        }

        /// <summary>
        /// 获取所有时间线列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<TimeLineDTO>> GetTimeLineListAsync()
        {
            var timeLines = await TimeLineRepository.GetModelsAsync(a => a.id != 0);
            return Mapper.Map<List<TimeLineDTO>>(timeLines.OrderByDescending(a => a.createDate).ToList());
        }
    }
}