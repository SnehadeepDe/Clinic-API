using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using System.Linq;

namespace ClinicAPI.Repository
{
    public class BaseRepository<T> where T : class
    {
        private readonly string _connectionString;

        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<T> GetAll(string query, int? id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return id.HasValue ? connection.Query<T>(query, new { Id = id }).ToList() : connection.Query<T>(query).ToList();
            }
        }
        public T GetById(string query, int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<T>(query, new { Id = id });
            }
        }
        public int Create(string query, T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>(query, entity);
            }
        }
        public int Create(string query, object parameters, CommandType commandType = CommandType.Text)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>(query, parameters, commandType: commandType);
            }
        }
        public void Update(string query, object entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, entity);
            }
        }
        public void Update(string query, object parameters, CommandType commandType = CommandType.Text)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, parameters, commandType: commandType);
            }
        }
        public void Delete(string query, int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, new { Id = id });
            }
        }
    }
}
