using DI_Practice.Models;

namespace DI_Practice.Services
{
    public interface IQueryUserService
    {
        List<QueryUser> GetAllQueries();
    }
}
