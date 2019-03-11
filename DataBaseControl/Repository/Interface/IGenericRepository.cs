using BlackJack.DataBaseAccess.Entities;
using System.Threading.Tasks;

namespace BlackJack.DataBaseAccess.Repository.Interface
{
    public interface IGenericRepository<T> where T : BasicEntities
    {
        Task<long> Create(T item);
        Task<T> FindById(long id);
        Task Remove(T item);
        Task Update(T item);
    }
}
