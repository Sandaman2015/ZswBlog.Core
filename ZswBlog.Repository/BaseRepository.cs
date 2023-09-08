using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZswBlog.Common.Exception;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, new() //泛型约束必须是实体
    {
        //private readonly DbContext DbContext = DbContextFactory.Create();
        //public ZswBlogContext _dbContext { get; set; }

        /// <summary>
        /// 采用属性注入的方式，共享单例操作上下文，而不通过DbFactory去创建
        /// </summary>
        public ZswBlogDbContext DbContext { get; set; }

        //public DbContext DbContext { get; set; }

        //public DbContext DbContext { get; set; }

        public virtual bool Add(T t)
        {
            DbContext.Set<T>().Add(t);
            return DbContext.SaveChanges() > 0;
        }

        public virtual bool AddList(IEnumerable<T> t)
        {
            DbContext.Set<T>().AddRange(t);
            return DbContext.SaveChanges() > 0;
        }

        public virtual bool Delete(T t)
        {
            //必须将给定实体附加到集的基础上下文中。也就是说，将实体以“未更改”的状态放置到上下文中，就好像从数据库读取了该实体一样。
            DbContext.Set<T>().Attach(t);
            DbContext.Set<T>().Remove(t);
            return DbContext.SaveChanges() > 0;
        }


        public virtual bool DeleteList(IEnumerable<T> t)
        {
            DbContext.Set<T>().RemoveRange(t);
            return DbContext.SaveChanges() > 0;
        }

        public virtual bool Update(T t)
        {
            DbContext.Set<T>().Update(t);
            return DbContext.SaveChanges() > 0;
        }

        public virtual bool UpdateList(IEnumerable<T> t)
        {
            DbContext.Set<IEnumerable<T>>().UpdateRange(new IEnumerable<T>[]
        {
                t
        });
            return DbContext.SaveChanges() > 0;
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
                throw new BusinessException("查询执行错误:"+ ex.Message, 500);
            }
        }

        public virtual T GetSingleModel(Expression<Func<T, bool>> whereLambda)
        {
            return  DbContext.Set<T>().Where(whereLambda).FirstOrDefault<T>();
        }

        public virtual int GetModelsCountByCondition(Expression<Func<T, bool>> whereLambda)
        {
            return whereLambda == null
                ? DbContext.Set<T>().Count()
                : DbContext.Set<T>().Where(whereLambda).Count();
        }

        public virtual PageEntity<T> GetModelsByPage<TType>(int pageSize, int pageIndex, bool isAsc, Expression<Func<T, TType>> orderByLambda, Expression<Func<T, bool>> whereLambda)
        {
            var result = DbContext.Set<T>().Where(whereLambda);
            var total = result.Count();
            var data = isAsc
                ? result.OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                : result.OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new PageEntity<T>(pageIndex, pageSize, total, data);
        }
    }
}