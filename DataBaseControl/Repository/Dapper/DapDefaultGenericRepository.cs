﻿using BlackJack.DataBaseAccess.Entities;
using BlackJack.DataBaseAccess.Repository.Interface;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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

        public async virtual Task Remove(T item)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                cn.Open();
                await Task.Run(() => cn.Execute($"DELETE FROM {_tableName} WHERE Id=@Id", new { Id = item.Id }));
            }
        }

        public async virtual Task<T> FindById(long id)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                return await Task.Run(()=> cn.Query<T>($"SELECT * FROM {_tableName} WHERE Id=@Id", new { Id = id }).SingleOrDefault());
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
                await Task.Run(() => cn.Update(item));
            }

            return;
        }
    }
}
