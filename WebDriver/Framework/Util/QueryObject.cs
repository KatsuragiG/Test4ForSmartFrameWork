using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Dapper;
using Newtonsoft.Json;
using WebdriverFramework.Framework.WebDriver;

namespace WebdriverFramework.Framework.Util
{
    /// <summary>
    /// For work with Database using Dapper micro-ORM
    /// </summary>
    public abstract class QueryObject
    {
        private readonly DbProviderFactory dbProviderFactory;

        /// <summary>
        /// constructor 
        /// </summary>
        protected QueryObject()
        {
            this.dbProviderFactory = DbProviderFactories.GetFactory(Configuration.Database.ProviderInvariantName);
        }

        /// <summary>
        /// execute query from file that marked as Embedded Resource with parameters or without and dynamic return 
        /// </summary>
        /// <param name="sqlFileName">name of file (*.sql) with SQL query</param>
        /// <param name="parameters">object with parameters for query</param>
        /// <returns></returns>
        protected IEnumerable<dynamic> ExecuteQueryFromFileWithParameters(string sqlFileName, object parameters)
        {
            return ExecuteQueryFromFileWithParameters(sqlFileName, parameters, null);
        }

        /// <summary>
        /// execute query from file that marked as Embedded Resource with parameters or without and dynamic return 
        /// </summary>
        /// <param name="sqlFileName">name of file (*.sql) with SQL query</param>
        /// <param name="parameters">object with parameters for query</param>
        /// <param name="connectionString">database connection string, by default get connection string from Configuration</param>
        /// <returns></returns>
        protected IEnumerable<dynamic> ExecuteQueryFromFileWithParameters(string sqlFileName, object parameters, string connectionString)
        {
            return this.ExecuteQueryFromFileWithParameters<dynamic>(sqlFileName, parameters, connectionString);
        }

        /// <summary>
        /// execute query from file that marked as Embedded Resource with parameters or without
        /// </summary>
        /// <typeparam name="TResult">type of result</typeparam>
        /// <param name="sqlFileName">name of file (*.sql) with SQL query</param>
        /// <param name="parameters">object with parameters for query</param>
        /// <returns></returns>
        protected IEnumerable<TResult> ExecuteQueryFromFileWithParameters<TResult>(string sqlFileName, object parameters)
        {
            return ExecuteQueryFromFileWithParameters<TResult>(sqlFileName, parameters, null);
        }

        /// <summary>
        /// execute query from file that marked as Embedded Resource with parameters or without
        /// </summary>
        /// <typeparam name="TResult">type of result</typeparam>
        /// <param name="sqlFileName">name of file (*.sql) with SQL query</param>
        /// <param name="parameters">object with parameters for query</param>
        /// <param name="connectionString">database connection string, by default get connection string from Configuration</param>
        /// <returns></returns>
        protected IEnumerable<TResult> ExecuteQueryFromFileWithParameters<TResult>(string sqlFileName, object parameters, string connectionString)
        {
            var queryString = this.GetQueryString(sqlFileName);
            return ExecuteQueryWithParameters<TResult>(queryString, parameters, connectionString);
        }

        /// <summary>
        /// execute query with parameters or without and dynamic return 
        /// </summary>
        /// <param name="sqlQuery">SQL query</param>
        /// <param name="parameters">object with parameters for query</param>
        /// <returns></returns>
        protected IEnumerable<dynamic> ExecuteQueryWithParameters(string sqlQuery, object parameters)
        {
            return ExecuteQueryWithParameters(sqlQuery, parameters, null);
        }

        /// <summary>
        /// execute query with parameters or without and dynamic return 
        /// </summary>
        /// <param name="sqlQuery">SQL query</param>
        /// <param name="parameters">object with parameters for query</param>
        /// <param name="connectionString">database connection string, by default get connection string from Configuration</param>
        /// <returns></returns>
        protected IEnumerable<dynamic> ExecuteQueryWithParameters(string sqlQuery, object parameters, string connectionString)
        {
            return ExecuteQueryWithParameters<dynamic>(sqlQuery, parameters, connectionString);
        }

        /// <summary>
        /// execute query with parameters or without
        /// </summary>
        /// <typeparam name="TResult">type of result</typeparam>
        /// <param name="sqlQuery">SQL query</param>
        /// <param name="parameters">object with parameters for query</param>
        /// <returns></returns>
        protected IEnumerable<TResult> ExecuteQueryWithParameters<TResult>(string sqlQuery, object parameters)
        {
            return ExecuteQueryWithParameters<TResult>(sqlQuery, parameters, null);
        }

        /// <summary>
        /// execute query with parameters or without
        /// </summary>
        /// <typeparam name="TResult">type of result</typeparam>
        /// <param name="sqlQuery">SQL query</param>
        /// <param name="parameters">object with parameters for query</param>
        /// <param name="connectionString">database connection string, by default get connection string from Configuration</param>
        /// <returns></returns>
        protected IEnumerable<TResult> ExecuteQueryWithParameters<TResult>(string sqlQuery, object parameters, string connectionString)
        {
            var databaseConnectionString = connectionString ?? Configuration.Database.ConnectionString;
            using (var connection = this.CreateConnection(databaseConnectionString))
            {
                connection.Open();
                Logger.Instance.Debug(
                    $"Executing SQL:\n {sqlQuery} \n with parameters: \n{JsonConvert.SerializeObject(parameters, Formatting.Indented)}");
                var result = connection.Query<TResult>(sqlQuery, parameters, commandTimeout: int.Parse(Configuration.QueryTimeout));
                Logger.Instance.Debug($"Result:\n {JsonConvert.SerializeObject(result, Formatting.Indented)}");
                return result;
            }
        }

        private IDbConnection CreateConnection(string connectionString)
        {
            var connection = this.dbProviderFactory.CreateConnection();
            if (connection != null)
            {
                connection.ConnectionString = connectionString;
            }
            return connection;
        }

        /// <summary>
        /// Get Query String from file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected string GetQueryString(string fileName)
        {
            var type = this.GetType();
            return EmbeddedResourceUtility.GetFileContentsFromNamespace(type.Assembly, type.Namespace, fileName);
        }
    }
}
