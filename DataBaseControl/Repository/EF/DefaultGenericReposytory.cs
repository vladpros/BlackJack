using BlackJackDataBaseAccess.Entities;
using BlackJackDataBaseAccess.Repository.Interface;
using System.Data.Entity;

namespace BlackJackDataBaseAccess.Repository
{
    public class DefaultGenericRepository<T> : IGenericRepository<T> where T : BasicEntities
    {
        private DbContext _context;
        private DbSet<T> _dbSet;

        public DefaultGenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public T FindById(long id)
        {
            return _dbSet.Find(id);
        }

        public long Create(T item)
        {
            long id = _dbSet.Add(item).Id;
            _context.SaveChanges();

            return id;
        }

        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(T item)
        {
            _dbSet.Remove(item);
            _context.SaveChanges();
        }

    }
}
