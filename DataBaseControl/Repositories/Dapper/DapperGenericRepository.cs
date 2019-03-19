using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repositories.Interfaces;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repositories.Dapper
{
    public class DapperGenericRepository<T> : IGenericRepository<T> where T : BasicEntitie
    {

        private readonly string _tableName;
        private readonly string _conString;

        public DapperGenericRepository(string tableName, string conString)
        {
            _conString = conString;
            _tableName = tableName;
        }

        public async virtual Task Remove(T item)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                cn.Open();
                await cn.ExecuteAsync($"DELETE FROM {_tableName} WHERE Id=@Id", new { Id = item.Id });
            }
        }

        public async virtual Task<T> FindById(long id)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                var result = (await cn.QueryAsync<T>($"SELECT * FROM {_tableName} WHERE Id=@Id", new { Id = id })).SingleOrDefault();
                return result;
            }         
        }

        public async virtual Task<long> Create(T item)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                item.Id = await cn.InsertAsync(item);
                return item.Id;
            }
        }

        public async virtual Task Update(T item)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                await cn.UpdateAsync(item);
            }
        }
    }
}
