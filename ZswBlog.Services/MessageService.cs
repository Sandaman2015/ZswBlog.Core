using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace Services
{
    public class MessageService : BaseService, IMessageService
    {
        public MessageService(IMessageRepository repository)
        {
            _repository = repository;
        }
        private readonly IMessageRepository _repository;

        public async Task<List<Message>> GetAllMessagesAsync()
        {
            return await Task.Run(() =>
            {
                List<Message> messages = _repository.GetModels(a => a.MessageId != 0).ToList();
                return messages;
            });
        }

        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            return await Task.Run(() =>
            {
                Message message = _repository.GetSingleModel(a => a.MessageId == messageId);
                return message;
            });
        }

        public async Task<List<Message>> GetMessagesByPageAsync(int limit, int pageIndex)
        {
            return await Task.Run(() =>
            {
                List<Message> messages = _repository.GetModelsByPage(limit, pageIndex, false, (a => a.MessageDate), (a => a.MessageId != 0), out int total).ToList();
                return messages;
            });
        }

        public async Task<List<Message>> GetMessagesByTargetIdAsync(int targetId)
        {
            return await Task.Run(() =>
            {
                List<Message> messages = _repository.GetModels(a => a.TargetId == targetId).OrderBy(a => a.MessageDate).ToList();
                return messages;
            });
        }

        public async Task<List<Message>> GetMessagesOnNotReplyAsync()
        {
            return await Task.Run(() =>
            {
                List<Message> messages = _repository.GetModels(a => a.TargetId == 0).ToList();
                return messages;
            });
        }

        public async Task<List<Message>> GetMessagesOnNotReplyAsyncByPageAsync(int limit, int pageIndex)
        {
            recursionMessages.Clear();
            return await Task.Run(() =>
            {
                List<Message> messages = _repository.GetModelsByPage(limit, pageIndex, false, (a => a.MessageId), (a => a.TargetId == 0), out int total).ToList();
                return messages;
            });
        }
        public async Task<bool> AddEntityAsync(Message t)
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
                Message message = _repository.GetSingleModel(a => a.MessageId == tId);
                return _repository.Delete(message);
            });
        }

        public Task<bool> AlterEntityAsync(Message t)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsExistsMessageOnNewestByUserId(Guid userId)
        {
            return await Task.Run(() =>
            {

                List<Message> messages = _repository.GetModels(a => a.UserId == userId).OrderByDescending(a => a.MessageDate).ToList();
                if (messages != null && messages.Count > 0)
                {
                    TimeSpan timeSpan = DateTime.Now - messages[0].MessageDate;
                    return timeSpan.TotalMinutes > 1;
                }
                else return true;
            });
        }
        private List<Message> recursionMessages = new List<Message>();
        public List<Message> GetMessagesByRecursion(int targetId)
        {
            List<Message> messages = _repository.GetModels(a => a.TargetId == targetId).ToList();
            if (messages != null && messages.Count() > 0)
            {
                foreach (var item in messages)
                {
                    recursionMessages.Add(item);
                }
                return GetMessagesByRecursion(messages[0].MessageId);
            }
            else
            {
                return recursionMessages;
            }
        }

        public bool ClearRecursionMessages()
        {
            recursionMessages.Clear();
            return recursionMessages.Count() == 0;
        }
    }
}
