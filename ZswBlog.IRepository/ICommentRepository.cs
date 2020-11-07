﻿using System.Collections.Generic;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IRepository
{
    public interface ICommentRepository : IBaseRepository<CommentEntity> {
        List<CommentDTO> GetCommentsRecursive(int targetId, int articleId);
    }
}
