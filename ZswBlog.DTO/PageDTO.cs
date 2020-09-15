using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.DTO
{
    /// <summary>
    /// 分页返回支持类
    /// </summary>
    /// <typeparam name="T">实体或DTO对象</typeparam>
    public class PageDTO<T> where T : class,new()
    {
        /// <summary>
        /// 填充数据对象
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        public PageDTO(int pageIndex, int pageSize, int count, List<T> data)
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
        /// 总数
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 对象集合
        /// </summary>
        public List<T> data { get; set; }

    }
}
