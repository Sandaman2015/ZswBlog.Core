using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public abstract class BaseService<T, D> : IBaseService<T> where T : class, new() where D : IBaseRepository<T>
    {
        public IBaseRepository<T> _repository { get; set; }

        public virtual bool AddEntity(T t)
        {
            return _repository.Add(t);
        }

        public virtual bool UpdateEntity(T t)
        {
            return _repository.Update(t);
        }
    }
}
