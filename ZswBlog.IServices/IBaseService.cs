using System;
using System.Threading.Tasks;

namespace ZswBlog.IServices
{
    public interface IBaseService<in T> where T : class, new()
    {
        Task<bool> AddEntityAsync(T t);
        Task<bool> UpdateEntityAsync(T t);
        Task<int> GetEntitiesCountAsync();
    }
}
