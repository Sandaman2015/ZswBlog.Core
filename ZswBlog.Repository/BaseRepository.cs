using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, new() //泛型约束必须是实体
    {
        //private readonly DbContext _dbContext = DbContextFactory.Create();
        //public ZswBlogContext _dbContext { get; set; }

        /// <summary>
        /// 采用属性注入的方式，共享单例操作上下文，而不通过DbFactory去创建
        /// </summary>
        public ZswBlogDbContext DbContext { get; set; }

        //public WritleDbContext WritleDbContext { get; set; }

        //public ReadDbContext DbContext { get; set; }

        public virtual async Task<bool> AddAsync(T t)
        {
            await DbContext.Set<T>().AddAsync(t);
            return await DbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> AddListAsync(IEnumerable<T> t)
        {
            await DbContext.Set<T>().AddRangeAsync(t);
            return await DbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> DeleteAsync(T t)
        {
            //必须将给定实体附加到集的基础上下文中。也就是说，将实体以“未更改”的状态放置到上下文中，就好像从数据库读取了该实体一样。
            DbContext.Set<T>().Attach(t);
            DbContext.Set<T>().Remove(t);
            return await DbContext.SaveChangesAsync() > 0;
        }


        public virtual async Task<bool> DeleteListAsync(IEnumerable<T> t)
        {
            DbContext.Set<T>().RemoveRange(t);
            return await DbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> UpdateAsync(T t)
        {
            DbContext.Set<T>().Update(t);
            return await DbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> UpdateListAsync(IEnumerable<T> t)
        {
            DbContext.Set<IEnumerable<T>>().UpdateRange(new IEnumerable<T>[]
        {
                t
        });
            return await DbContext.SaveChangesAsync() > 0;
        }

        public virtual IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
            return DbContext.Set<T>().Where(whereLambda);
        }

        public virtual  IQueryable<T> GetModelsByPage<TType>(int pageSize, int pageIndex, bool isAsc,
            Expression<Func<T, TType>> orderByLambda, Expression<Func<T, bool>> whereLambda, out int total)
        {
            var result = DbContext.Set<T>().Where(whereLambda);
            try
            {
                total = result.Count();
                return isAsc
                    ? result.OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                    : result.OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            catch (Exception ex) {
                total = 0;
                return isAsc
                    ? result.OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                    : result.OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
        }

        public virtual async Task<T> GetSingleModelAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await DbContext.Set<T>().Where(whereLambda).FirstOrDefaultAsync<T>();
        }

        public virtual async Task<int> GetModelsCountByConditionAsync(Expression<Func<T, bool>> whereLambda)
        {
            return whereLambda == null
                ? await DbContext.Set<T>().CountAsync()
                : await DbContext.Set<T>().Where(whereLambda).CountAsync();
        }

        public virtual async Task<PageEntity<T>> GetModelsByPageAsync<TType>(int pageSize, int pageIndex, bool isAsc, Expression<Func<T, TType>> orderByLambda, Expression<Func<T, bool>> whereLambda)
        {
            return await Task.Run(() =>
            {
                var result = DbContext.Set<T>().Where(whereLambda);
                var total = result.AsNoTracking().Count();
                var data = isAsc
                    ? result.OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                    : result.OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return new PageEntity<T>(pageIndex, pageSize, total, data);
            });
        }
    }
}