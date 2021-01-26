using System;
using System.Threading.Tasks;

namespace ZswBlog.IServices
{
    public interface IBaseService<in T> where T : class, new()
    {
        //Task<IEnumerable<T>> GetTaskEntitiesByPage(int limit, int pageIndex, out int total);
        //Task<T> GetEntityById(int id);
        Task<bool> AddEntityAsync(T t);
        //Task<bool> RemoveEntityByIdAsync(int tId);
        Task<bool> UpdateEntityAsync(T t);
        Task<int> GetEntitiesCountAsync();
        //Task<Boolean> EntityAsync(T t);
    }
}
