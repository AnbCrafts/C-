using ConsoleApp5.Helpers;
using ConsoleApp5.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp5.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly SqlHelper _sqlHelper;

        protected BaseRepository(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public abstract Task CreateAsync(T entity);

        public abstract Task UpdateAsync(T entity);

        public abstract Task DeleteAsync(int id);

        public abstract Task<T> GetByIdAsync(int id);

        public abstract Task<IEnumerable<T>> GetAllAsync();
    }
}
