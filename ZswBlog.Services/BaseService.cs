using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZswBlog.Common.AopConfig;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    [Intercept(typeof(EnableTransaction))]
    public abstract class BaseService<T, D> : IBaseService<T> where T : class, new() where D : IBaseRepository<T>
    {
        public IBaseRepository<T> Repository { get; set; }

        public virtual Task<bool> AddEntityAsync(T t)
        {
            return Repository.AddAsync(t);
        }

        public Task<int> GetEntitiesCountAsync()
        {
            return Repository.GetModelsCountByConditionAsync(null);
        }

        public virtual Task<bool> UpdateEntityAsync(T t)
        {
            return Repository.UpdateAsync(t);
        }
    }
}
