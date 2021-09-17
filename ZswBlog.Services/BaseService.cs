using System.Threading.Tasks;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public abstract class BaseService<T, D> : IBaseService<T> where T : class, new() where D : IBaseRepository<T>
    {
        public D Repository { get; set; }

        public virtual bool AddEntity(T t)
        {
            return Repository.Add(t);
        }

        public virtual int GetEntitiesCount()
        {
            return Repository.GetModelsCountByCondition(null);
        }

        public virtual bool UpdateEntity(T t)
        {
            return Repository.Update(t);
        }
    }
}
