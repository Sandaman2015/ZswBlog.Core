using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace Services
{
    public class CommentService : BaseService, ICommentService
    {
        public CommentService(ICommentRepository repository)
        {
            _repository = repository;
        }
        private ICommentRepository _repository;
        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            return await Task.Run(() =>
            {
                Comment comment = _repository.GetSingleModel(a => a.CommentId == commentId);
                return comment;
            });
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await Task.Run(() =>
            {
                List<Comment> comment = _repository.GetModels(a => a.CommentId != 0).ToList();
                return comment;
            });
        }

        public async Task<List<Comment>> GetCommentsByTargetIdAsync(int targetId)
        {
            return await Task.Run(() =>
            {
                List<Comment> comment = _repository.GetModels(a => a.TargetId == targetId).ToList();
                return comment;
            });
        }

        public async Task<List<Comment>> GetCommentsOnNotReplyAsyncByPageAsync(int articleId, int limit, int pageIndex)
        {
            return await Task.Run(() =>
            {
                List<Comment> comments = _repository.GetModelsByPage(limit, pageIndex, false, (a => a.CommentId), (a => a.TargetId == 0 && a.ArticleId == articleId), out int total).ToList();
                return comments;
            });
        }

        public async Task<bool> AddEntityAsync(Comment t)
        {
            return await Task.Run(() =>
            {
                return _repository.Add(t);
            });
        }

        public async Task<bool> RemoveEntityAsync(int tId)
        {
            return await Task.Run(() =>
            {
                Comment comment = _repository.GetSingleModel(a => a.CommentId == tId);
                return _repository.Delete(comment);
            });
        }

        public Task<bool> AlterEntityAsync(Comment t)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Comment>> GetCommentsOnNotReplyAsync(int articleId)
        {
            return await Task.Run(() =>
            {
                List<Comment> comments = _repository.GetModels(a => a.TargetId == 0 && a.ArticleId == articleId).ToList();
                return comments;
            });
        }

        public async Task<bool> IsExistsCommentOnNewestByUserId(Guid userId)
        {
            return await Task.Run(() =>
            {
                List<Comment> comments = _repository.GetModels(a => a.UserId == userId).OrderByDescending(a => a.CommentDate).ToList();
                if (comments != null && comments.Count > 0)
                {
                    TimeSpan timeSpan = DateTime.Now - comments[0].CommentDate;
                    return timeSpan.TotalMinutes > 1;
                }
                else return true;
            });
        }
        //每当进行分页查询时，都将该列表清空为了重新赋值
        private List<Comment> recursionComments = new List<Comment>();
        public List<Comment> GetCommentsByRecursion(int targetId)
        {
            List<Comment> comments = _repository.GetModels(a => a.TargetId == targetId).ToList();
            if (comments != null && comments.Count > 0)
            {
                foreach (var item in comments)
                {
                    recursionComments.Add(item);
                }
                return GetCommentsByRecursion(comments[0].CommentId);
            }
            else
            {
                return recursionComments;
            }
        }

        public bool ClearRecursionComments()
        {
            recursionComments.Clear();
            return recursionComments.Count() == 0;
        }
    }
}
