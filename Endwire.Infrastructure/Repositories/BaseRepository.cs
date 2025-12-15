using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EndWire.Domain;
using EndWire.Domain.ConfigurationOptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EndWire.API.Repositories
{
    public class BaseRepository<T>
            where T : DbContext
    {
        protected readonly T Context;
        protected readonly IConfiguration Configuration;

        public BaseRepository(T context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public BaseRepository(T context, IConfiguration configuration)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        protected async Task CallStoredProcedureAsync(string spName, object spData)
        {
            var sqlConnectionString = Context.Database.GetDbConnection().ConnectionString;

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                await connection.QueryAsync(spName, spData, commandType: CommandType.StoredProcedure);
            }
        }

        protected async Task CallStoredProcedureAsync(StoredProcedure storedProcedure, object spData)
        {
            var sqlConnectionString = GetConnectionString(storedProcedure);

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(storedProcedure.Name, spData, commandType: CommandType.StoredProcedure);
            }
        }

        protected async Task<Type> CallStoredProcedureWithResultAsync<Type>(string spName, object spData)
        {
            var sqlConnectionString = Context.Database.GetDbConnection().ConnectionString;

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<Type>(spName, spData, commandType: CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
        }

        protected async Task<Type> CallStoredProcedureWithResultAsync<Type>(StoredProcedure storedProcedure, object spData)
        {
            var sqlConnectionString = GetConnectionString(storedProcedure);

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<Type>(storedProcedure.Name, spData, commandTimeout: storedProcedure.Timeout, commandType: CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
        }

        protected async Task<List<Type>> CallStoredProcedureWithResultsAsync<Type>(StoredProcedure storedProcedure, object spData)
        {
            var sqlConnectionString = GetConnectionString(storedProcedure);

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var results = await connection.QueryAsync<Type>(storedProcedure.Name, spData, commandTimeout: storedProcedure.Timeout, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
        }

        protected async Task<List<Type>> CallStoredProcedureWithResultsAsync<Type>(string storedProcedure, object spData)
        {
            var sqlConnectionString = Context.Database.GetDbConnection().ConnectionString;

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var results = await connection.QueryAsync<Type>(storedProcedure, spData, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
        }

        protected string GetConnectionString(StoredProcedure storedProcedure)
        {
            if (Configuration == null)
            {
                throw new InvalidOperationException("Base Repository must have IConfiguration injected");
            }

            if (storedProcedure == null || string.IsNullOrEmpty(storedProcedure.Database))
            {
                throw new ArgumentNullException(nameof(storedProcedure), "Stored Procedure object must exist in StoredProcedures.json");
            }

            return Configuration[$"ConnectionString:{storedProcedure.Database}"];
        }

        /*protected DataTable GenerateDataTableFromArray(int[] array)
        {
            if (array == null)
            {
                array = new int[0];
            }

            return array.Select(id => new CommonTable { Id = id }).ToList().ToDataTable();
        }

        protected DataTable GenerateDataTableFromArray(long[] array)
        {
            if (array == null)
            {
                array = new long[0];
            }

            return array.Select(id => new CommonTable { Id = id }).ToList().ToDataTable();
        }*/


        protected async Task<List<TOut>> QueryAsync<TOut>(string query, object data)
        {
            await using SqlConnection connection = CreateConnection();
            connection.Open();

            return (await connection.QueryAsync<TOut>(query, data)).ToList();
        }

        protected async Task<IEnumerable<TOut>> QueryAsync<TOut>(string query)
        {
            await using SqlConnection connection = CreateConnection();
            connection.Open();

            return (await connection.QueryAsync<TOut>(query)).ToList();
        }

        protected async Task<TOut> QuerySingleAsync<TOut>(string query, object data)
        {
            await using SqlConnection connection = CreateConnection();
            connection.Open();

            return await connection.QuerySingleAsync<TOut>(query, data);
        }

        protected async Task<TOut> QueryFirstOrDefaultAsync<TOut>(string query, object data)
        {
            await using SqlConnection connection = CreateConnection();
            connection.Open();

            return await connection.QueryFirstOrDefaultAsync<TOut>(query, data);
        }

        protected async Task<TOut> ExecuteScalarAsync<TOut>(string query, object data)
        {
            await using SqlConnection connection = CreateConnection();
            connection.Open();

            return await connection.ExecuteScalarAsync<TOut>(query, data);
        }

        protected async Task<int> ExecuteAsync(string query, object data)
        {
            await using SqlConnection connection = CreateConnection();
            connection.Open();

            return await connection.ExecuteAsync(query, data);
        }

        protected SqlConnection CreateConnection()
        {
            string connectionString = Context.Database.GetDbConnection().ConnectionString;

            return new SqlConnection(connectionString);
        }
    }
}
