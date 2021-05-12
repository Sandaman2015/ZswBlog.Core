using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class TravelFileAttachmentService : BaseService<TravelFileAttachmentEntity, ITravelFileAttachmentRepository>, ITravelFileAttachmentService
    {
        public ITravelFileAttachmentRepository travelFileAttachmentRepository { get; set; }
        public IFileAttachmentRepository fileAttachmentRepository { get; set; }
        public IMapper Mapper { get; set; }

        public async Task<List<FileAttachmentEntity>> GetTravelFileListByTravelId(int travelId)
        {
            var travelImgList =  travelFileAttachmentRepository.GetModels(a => a.travelId == travelId);
            var imgList = new List<FileAttachmentEntity>();
            foreach (var item in travelImgList) {
                FileAttachmentEntity fileAttachment = await fileAttachmentRepository.GetSingleModelAsync(a => a.id == item.fileAttachmentId);
                imgList.Add(fileAttachment);
            }
            return imgList;
        }

        public virtual async Task<bool> RemoveAllFileRelationAsync(int fileAttachmentId)
        {
            var travelImgList = travelFileAttachmentRepository.GetModels(a => a.fileAttachmentId == fileAttachmentId);
            var queryList = await travelImgList.ToListAsync();
            return await travelFileAttachmentRepository.DeleteListAsync(queryList);
        }

        public async Task<bool> RemoveAllTravelRelationAsync(int travelId)
        {
            var travelImgList = travelFileAttachmentRepository.GetModels(a => a.travelId == travelId);
            return await travelFileAttachmentRepository.DeleteListAsync(travelImgList);
        }
    }
}
