using System.Threading.Tasks;
using Refit;

namespace resilience.demo
{
    public interface IGithubApi
    {
        [Get("/users/{username}")]
        Task<GithubUser> User([AliasAs("username")] string userName);
    }
}