using Microsoft.AspNetCore.Mvc;
using Nacos.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZswBlog.Core.Controllers
{
    [ApiController]
    public class NacosTestController
    {
        private readonly INacosServerManager _serverManager;

        public NacosTestController(INacosServerManager serverManager)
        {
            _serverManager = serverManager;
        }

        [HttpGet("/api/nacos/get/user/info")]
        public async Task<ActionResult<string>> Test()
        {
            // need to know the service name.
            // support WeightRandom and WeightRoundRobin.
            var baseUrl = await _serverManager.GetServerAsync("marvel-middle-server").;

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return "empty";
            }

            var url = $"{baseUrl}/middle/api/user/userInfo";

            using (HttpClient client = new HttpClient())
            {
                Console.WriteLine(client.BaseAddress.Host);
                Console.WriteLine(client.BaseAddress);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOlsibWFydmVsLWhyLXNlcnZlciIsIm1hcnZlbC1wcm9qZWN0LXNlcnZlciIsIm1hcnZlbC1hdXRoLXNlcnZlciIsIm1hcnZlbC1zdXBlcnZpc2lvbi1pbnNwZWN0aW9uIiwibWFydmVsLW1pZGRsZS1zZXJ2ZXIiLCJtYXJ2ZWwtb3Blbi1hcGkiLCJtYXJ2ZWwtdmlkZW8tbW9uaXRvciIsIm1hcnZlbC1tZXNzYWdlLXNlcnZlciJdLCJleHAiOjE2MTAyMDQzMjcsInVzZXJfbmFtZSI6ImFkbWluIiwianRpIjoiZjIyNDA1YjEtYmMwMC00NjM3LTk5NWItNTU3ZGZkNmZiNWJjIiwiY2xpZW50X2lkIjoibWlkZGxlIiwic2NvcGUiOlsicmVhZCIsIndyaXRlIl19.SOXSizGwbEP5gToC0vZMQHFptV-0HLqRW_Wwlajnjhs");
                var result = await client.GetAsync(url);
                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}
