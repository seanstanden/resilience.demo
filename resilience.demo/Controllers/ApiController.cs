using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace resilience.demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly IGithubApi githubApi;

        public ApiController(IHttpClientFactory httpClientFactory)
        {
            githubApi = RestService.For<IGithubApi>(httpClientFactory.CreateClient(nameof(IGithubApi)));
        }

        [HttpGet]
        public async Task<GithubUser> Get()
        {
            var result = await githubApi.User("octocat");
            return result;
        }
    }
}