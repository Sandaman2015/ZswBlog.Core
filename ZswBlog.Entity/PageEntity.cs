using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageEntity<T> : BaseEntity where T : class, new()
    {
        /// <summary>
        /// 分页对象构造函数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        public PageEntity(int pageIndex, int pageSize, int count, IQueryable<T> data)
        {
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
            this.count = count;
            this.data = data;
        }

        /// <summary>
        /// 页数
        /// </summary>
        public int pageIndex { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 分页列表
        /// </summary>
        public IQueryable<T> data { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int count { get; set; }
    }
}
