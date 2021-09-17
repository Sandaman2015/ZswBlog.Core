using System;
using System.Threading.Tasks;

namespace ZswBlog.IServices
{
    public interface IBaseService<in T> where T : class, new()
    {
        bool AddEntity(T t);
        bool UpdateEntity(T t);
        int GetEntitiesCount();
    }
}
