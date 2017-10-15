using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DoLess.Rest.ConsoleSample.GitHub.V3.Entities;

namespace DoLess.Rest.ConsoleSample.GitHub.V3
{
    [Header("Accept", "application/vnd.github.v3+json")]
    [Header("User-Agent", "DoLess.Rest Sample")]
    public interface IGitHubApi
    {
        [Get("/users{/username}")]
        Task<User> GetUserAsync(string username);

        [Get("/orgs{/org}/members{?filter,role}")]
        Task<IReadOnlyList<User>> GetOrganizationMembersAsync(string org, string filter = null, string role = null);

        // The '+' character has to be encoded into '%2B' if we follow the RFC 6570, but the GitHub API does not allow this.
        // So we have to do this trick in order to send thr right url.
        [Get("/search/users?q={+q}{&sort,order}")]
        Task<SearchResult<User>> FindUsersAsync([Name("q")]string query, string sort = null, string order = null);
    }
}
