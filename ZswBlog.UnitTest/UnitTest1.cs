using NUnit.Framework;
using ZswBlog.DTO;
using ZswBlog.IServices;
using ZswBlog.Services;

namespace ZswBlog.UnitTest
{
    public class Tests
    {

        private readonly ICommentService commentService;

        public Tests(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //ICommentService commentService = new CommentService();
            PageDTO<CommentTreeDTO> pageDTO = commentService.GetCommentsByRecursion(3, 1);
            System.Console.WriteLine(pageDTO.count);
            System.Console.WriteLine(pageDTO.data);
            System.Console.WriteLine(pageDTO.pageIndex);
            System.Console.WriteLine(pageDTO.pageSize);
        }
    }
}