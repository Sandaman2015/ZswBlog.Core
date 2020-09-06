using System;
using System.Threading.Tasks;

namespace ZswBlog.IServices
{
    public interface IBaseService<T> where T : class, new()
    {
        //Task<IEnumerable<T>> GetTaskEntitiesByPage(int limit, int pageIndex, out int total);
        //Task<T> GetEntitiesById(int id);
        Task<Boolean> AddEntityAsync(T t);
        Task<Boolean> RemoveEntityAsync(int tId);
        Task<Boolean> AlterEntityAsync(T t);
        //Task<Boolean> EntityAsync(T t);
    }
}
