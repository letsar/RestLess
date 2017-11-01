using System.Threading.Tasks;
using RestLess.Tests.Entities;

namespace RestLess.Tests.Interfaces
{
    public interface IApi09
    {
        [Get("api/people")]
        Task<PagedResponse<Person>> GetPagedPeopleAsync();
    }
}
