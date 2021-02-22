using System.Threading.Tasks;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public abstract class BaseService<T, D> : IBaseService<T> where T : class, new() where D : IBaseRepository<T>
    {
        public D Repository { get; set; }

        public virtual async Task<bool> AddEntityAsync(T t)
        {
            return await Repository.AddAsync(t);
        }

        public virtual async Task<int> GetEntitiesCountAsync()
        {
            return await Repository.GetModelsCountByConditionAsync(null);
        }

        public virtual async Task<bool> UpdateEntityAsync(T t)
        {
            return await Repository.UpdateAsync(t);
        }
    }
}
