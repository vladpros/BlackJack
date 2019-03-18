using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repository.Interface;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repository
{
    public class DefaultGenericRepository<T> : IGenericRepository<T> where T : BasicEntities
    {
        private BlackJackContext _context;
        private DbSet<T> _dbSet;

        public DefaultGenericRepository(BlackJackContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> FindById(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<long> Create(T item)
        {
            var iten = await Task.Run( () => _dbSet.Add(item));
            await Task.Run(() => _context.SaveChanges());

            return  iten.Id;
        }

        public async Task Update(T item)
        {
            await Task.Run(() => _context.Entry(item).State = EntityState.Modified);
            await Task.Run(() => _context.SaveChanges());

            return;
        }

        public async Task Remove(T item)
        {
            await Task.Run(() => _dbSet.Remove(item));
            await Task.Run(() => _context.SaveChanges());
        }

    }
}
