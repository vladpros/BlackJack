using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repositories.Interfaces;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repositories.EF
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BasicEntitie
    {
        private BlackJackContext _context;
        private DbSet<T> _dbSet;

        public GenericRepository(BlackJackContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> FindById(long id)
        {
            var result = await _dbSet.FindAsync(id);
            return result;
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
        }

        public async Task Remove(T item)
        {
            await Task.Run(() => _dbSet.Remove(item));
            await Task.Run(() => _context.SaveChanges());
        }

    }
}
