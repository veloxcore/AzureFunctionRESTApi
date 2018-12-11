using Rest.Data.Utils;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Rest.Data.Infrastructure
{
    //public interface IRepository<T> where T:class
    //{
    //    int ProcessCommand(string commandQuery, IDictionary<string, object> parameters = null);
    //    DataSet ProcessQuery(string query, IDictionary<string, object> parameters = null);
    //    object ProcessScalar(string query, IDictionary<string, object> parameters = null);

    //    IEnumerable<T> GetList(string query, IDictionary<string, object> parameters = null);
    //    IEnumerable<K> GetList<K>(string query, IDictionary<string, object> parameters = null, bool ignoreMissingFields = false);

    //    T Get(string query, IDictionary<string, object> parameters = null, bool ignoreMissingFields = false);
    //    IDictionary<string, object> ConvertToSQLParams<T>(List<T> paramValues, string suffix = "param");

    //    IRequest PrepareRequest(string query, IDictionary<string, object> parameters = null);
    //}
    public abstract class Repository<T>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the query processor.
        /// </summary>
        /// <value>
        /// The query processor.
        /// </value>
        public IDbQueryProcessor QueryProcessor { get; set; }

        #endregion

        public Repository(IDbQueryProcessor dbQueryProcessor)
        {
            QueryProcessor = dbQueryProcessor;
        }

        #region Protected Methods

        /// <summary>
        /// Requests the command.
        /// </summary>
        /// <param name="commandQuery">The command query.</param>
        /// <returns></returns>
        protected int ProcessCommand(string commandQuery, IDictionary<string, object> parameters = null)
        {
            IRequest req = PrepareRequest(commandQuery, parameters);
            return QueryProcessor.ProcessCommand(req);
        }

        protected async Task<int> ProcessCommandAsync(string commandQuery, IDictionary<string, object> parameters = null)
        {
            IRequest req = PrepareRequest(commandQuery, parameters);
            return await Task.Run<int>(() =>
            {
                return QueryProcessor.ProcessCommand(req);
            });
        }

        /// <summary>
        /// Requests the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected DataSet ProcessQuery(string query, IDictionary<string, object> parameters = null)
        {
            IRequest req = PrepareRequest(query, parameters);
            var ds = QueryProcessor.ProcessQuery(req);
            return ds;
        }

        /// <summary>
        /// Requests the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected async Task<DataSet> ProcessQueryAsync(string query, IDictionary<string, object> parameters = null)
        {
            IRequest req = PrepareRequest(query, parameters);
            return await Task.Run<DataSet>(() =>
            {
                return QueryProcessor.ProcessQuery(req);
            });
        }

        /// <summary>
        /// Gets the scalar value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected object ProcessScalar(string query, IDictionary<string, object> parameters = null)
        {
            IRequest req = PrepareRequest(query, parameters);
            var scallerValue = QueryProcessor.ProcessScalar(req);

            return scallerValue;
        }

        /// <summary>
        /// Gets the scalar value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected async Task<object> ProcessScalarAsync(string query, IDictionary<string, object> parameters = null)
        {
            IRequest req = PrepareRequest(query, parameters);
            var scallerValue = QueryProcessor.ProcessScalar(req);
            return await Task.Run<object>(() =>
            {
                return QueryProcessor.ProcessQuery(req);
            });
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Strongly typed list</returns>
        protected IEnumerable<T> GetList(string query, IDictionary<string, object> parameters = null)
        {
            return GetList<T>(query, parameters);
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Strongly typed list</returns>
        protected async Task<IEnumerable<T>> GetListAsync(string query, IDictionary<string, object> parameters = null)
        {
            return await GetAsync<T>(query, parameters);
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <typeparam name="K">Type of list to get back.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// List of strongly type K
        /// </returns>
        protected IEnumerable<K> GetList<K>(string query, IDictionary<string, object> parameters = null, bool ignoreMissingFields = false)
        {
            var ds = ProcessQuery(query, parameters);

            var list = ds.Tables[0].ToList<K>(ignoreMissingFields);
            return list;
        }

         /// <summary>
        /// Processes the request.
        /// </summary>
        /// <typeparam name="K">Type of list to get back.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// List of strongly type K
        /// </returns>
        protected async Task<IEnumerable<K>> GetAsync<K>(string query, IDictionary<string, object> parameters = null, bool ignoreMissingFields = false)
        {
            var ds = await ProcessQueryAsync(query, parameters);

            var list = ds.Tables[0].ToList<K>(ignoreMissingFields);
            return list;
        }

        /// <summary>
        /// Gets item by the specified query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected T Get(string query, IDictionary<string, object> parameters = null, bool ignoreMissingFields = false)
        {
            var item = GetList<T>(query, parameters, ignoreMissingFields);
            if (item.Any())
                return item.FirstOrDefault();
            else
                return default(T);
        }

        /// <summary>
        /// Gets item by the specified query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected async Task<T> GetAsync(string query, IDictionary<string, object> parameters = null, bool ignoreMissingFields = false)
        {
            var item = await GetAsync<T>(query, parameters, ignoreMissingFields);
            if (item.Any())
                return item.FirstOrDefault();
            else
                return default(T);
        }

        /// <summary>
        /// Converts to SQL parameters, which can be used with IN clause
        /// </summary>
        /// <param name="paramValues">The param values.</param>
        /// <returns>Dictionary of parameter which can be directly passed to execute query.</returns>
        protected IDictionary<string, object> ConvertToSQLParams<T>(List<T> paramValues, string suffix = "param")
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            suffix = "?" + suffix;
            for (int i = 0; i < paramValues.Count; i++)
            {
                parameters.Add(suffix + i.ToString(), paramValues[i]);
            }

            return parameters;
        }


        #endregion

        #region Private Methods

        private IRequest PrepareRequest(string query, IDictionary<string, object> parameters = null)
        {
            IRequest req = QueryProcessor.NewRequest();

            if (parameters != null)
            {
                req.RequestParameters = new List<SqlParameter>(parameters.Count());
                foreach (var item in parameters)
                {
                    SqlParameter userParam = new SqlParameter(item.Key, item.Value);
                    req.RequestParameters.Add(userParam);
                }
            }
            req.Commands = query;

            return req;
        }

        protected abstract object[] GetKey(T enity);
        #endregion

    }
}
