using AutoMapper;
using NUnit.Framework;
using ZswBlog.DTO;
using ZswBlog.IServices;
using ZswBlog.Repository;
using ZswBlog.Services;

namespace ZswBlog.UnitTest
{
    public class Tests
    {

        private readonly IMessageService messageService;

        public Tests(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            PageDTO<MessageTreeDTO> pageDTO = messageService.GetMessagesByRecursion(3, 1);
            System.Console.WriteLine(pageDTO.count);
            System.Console.WriteLine(pageDTO.data);
            System.Console.WriteLine(pageDTO.pageIndex);
            System.Console.WriteLine(pageDTO.pageSize);
        }
    }
}