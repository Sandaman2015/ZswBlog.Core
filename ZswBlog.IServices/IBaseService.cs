﻿using System;
using System.Threading.Tasks;

namespace ZswBlog.IServices
{
    public interface IBaseService<T> where T : class, new()
    {
        //Task<IEnumerable<T>> GetTaskEntitiesByPage(int limit, int pageIndex, out int total);
        //Task<T> GetEntityById(int id);
        bool AddEntityAsync(T t);
        //Task<bool> RemoveEntityByIdAsync(int tId);
        bool UpdateEntityAsync(T t);
        //Task<Boolean> EntityAsync(T t);
    }
}
