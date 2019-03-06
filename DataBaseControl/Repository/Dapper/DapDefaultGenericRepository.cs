using BlackJackDataBaseAccess.Entities;
using BlackJackDataBaseAccess.Repository.Interface;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataBaseControl.Repository.Dapper
{
    public class DapDefaultGenericRepository<T> : IGenericRepository<T> where T : BasicEntities
    {

        private readonly string _tableName;
        private readonly string _conString;

        public DapDefaultGenericRepository(string tableName)
        {
            _conString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            _tableName = tableName;
        }

        public virtual void Remove(T item)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                cn.Open();
                cn.Execute($"DELETE FROM {_tableName} WHERE Id=@Id", new { Id = item.Id });
            }
        }

        public virtual T FindById(long id)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                return cn.Query<T>($"SELECT * FROM {_tableName} WHERE Id=@Id", new { Id = id }).SingleOrDefault();
            }         
        }

        public virtual long Create(T item)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                item.Id = cn.Insert(item);
                return item.Id;
            }
        }

        public virtual void Update(T item)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                cn.Update(item);
            }
        }
    }
}
