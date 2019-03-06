using DataBaseControl.Entities;
using System.Data.Entity;

namespace DataBaseControl.Repository
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

        public void Create(T item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
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
