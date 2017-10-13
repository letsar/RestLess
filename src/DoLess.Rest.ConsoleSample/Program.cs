using System;
using System.Threading.Tasks;

namespace DoLess.Rest.ConsoleSample
{
    public class User
    {
        public string Login { get; set; }

        public string Url { get; set; }
    }

    [Header("User-Agent", "DoLess.Rest")]
    public interface IGitHubApi
    {
        [Get("users{/userId}")]
        Task<User> GetUserAsync(string userId);
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            IGitHubApi gitHubApi = RestClient.For<IGitHubApi>("https://api.github.com", new JsonRestSettings());

            User user = await gitHubApi.GetUserAsync("lestar");

            Console.WriteLine($"Login: {user.Login}, Url:{user.Url}");
            Console.Read();
        }
    }
}
