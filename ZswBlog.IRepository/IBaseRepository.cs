using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IRepository
{
    /// <summary>
    /// 数据访问接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : class, new()//泛型约束
    {
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<bool> AddAsync(T t);

        /// <summary>
        /// 添加可遍历的对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<bool> AddListAsync(IEnumerable<T> t);

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(T t);

        /// <summary>
        /// 删除可迭代的对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<bool> DeleteListAsync(IEnumerable<T> t);

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T t);

        /// <summary>
        /// 更新可迭代的对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<bool> UpdateListAsync(IEnumerable<T> t);

        /// <summary>
        /// 查询当前实体数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<T> GetSingleModelAsync(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="whereLambda">GetModels里面的是一个实体类=>实体类的成员属性判断后返回tolist<实体成员></param>
        /// <returns></returns>
        IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 返回一个查询后的列表
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="isAsc">升降序</param>
        /// <param name="orderByLambda">排序条件的lambda表达式</param>
        /// <param name="whereLambda">查询条件lambda表达式</param>
        /// <param name="total"></param>
        /// <returns>返回一个排序后查询的列表和总计</returns>
        IQueryable<T> GetModelsByPage<TType>(int pageSize, int pageIndex, bool isAsc, Expression<Func<T, TType>> orderByLambda, Expression<Func<T, bool>> whereLambda, out int total);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        Task<PageEntity<T>> GetModelsByPageAsync<TType>(int pageSize, int pageIndex, bool isAsc, Expression<Func<T, TType>> orderByLambda, Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 根据条件获取实体数量
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<int> GetModelsCountByConditionAsync(Expression<Func<T, bool>> whereLambda);
    }
}
