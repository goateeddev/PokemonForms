using Dapper;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TradeUB.Infrastructure.DataAccess.Database
{
    public sealed class DbClient
    {
        private readonly ILogger<DbClient> _logger;
        private readonly MySqlConnection _mySqlConnection;

        public DbClient(ILogger<DbClient> logger, MySqlConnection mySqlConnection)
        {
            _logger = logger;
            _mySqlConnection = mySqlConnection;
        }

        public List<T> Query<T>(string sql, object parameters)
        {
            try
            {
                _logger.LogInformation("[{MethodName}] A query to the database was made: {sql}", nameof(Query), sql);
                using var connection = _mySqlConnection;
                var results = connection.Query<T>(sql, parameters);
                return results == null ? new List<T>(0) : results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{MethodName}] An error occurred while querying the database.", nameof(Query));
                return default;
            }
        }

        public int Execute(string sql, object parameters)
        {
            try
            {
                _logger.LogInformation("[{MethodName}] A query to the database was executed: {sql}", nameof(Execute), sql);
                using var connection = _mySqlConnection;
                var affectedRows = connection.Execute(sql, parameters);
                return affectedRows;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{MethodName}] An error occurred executing an sql query.", nameof(Execute));
                return default;
            }
        }
    }
}
