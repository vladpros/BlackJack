using BlackJack.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BasicEntity
    {
        Task<long> Create(T item);
        Task Create(IEnumerable<T> item);
        Task<T> FindById(long id);
        Task Remove(T item);
        Task Update(T item);
    }
}
