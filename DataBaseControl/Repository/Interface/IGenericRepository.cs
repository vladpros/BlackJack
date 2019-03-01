
namespace DataBaseControl.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        void Create(T item);
        T FindById(long id);
        void Remove(T item);
        void Update(T item);
    }
}
