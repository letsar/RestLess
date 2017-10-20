using System;
using System.Net.Http;
using System.Threading.Tasks;
using RestLess.ConsoleSample.GitHub.V3;
using RestLess.ConsoleSample.GitHub.V3.Entities;

namespace RestLess.ConsoleSample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IGitHubApi gitHubApi = RestClient.For<IGitHubApi>("https://api.github.com", new JsonRestSettings()
            {
                HttpMessageHandlerFactory = () => new DelegatingLoggingHandler(new HttpClientHandler())
            });

            User user = await gitHubApi.GetUserAsync("lestar");
            Console.WriteLine($"Login: {user.Login}, Url:{user.Url}");

            SearchResult<User> results = await gitHubApi.FindUsersAsync("tom+repos:<42+followers:<1000");

            Console.Read();
        }
    }
}
