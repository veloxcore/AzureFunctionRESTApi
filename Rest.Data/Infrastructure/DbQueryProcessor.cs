using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Rest.Data.Infrastructure
{
    /// <summary>
    /// Database query processor, is implementation to process, execute and return result for database query.
    /// </summary>
    public interface IDbQueryProcessor
    {
        /// <summary>
        /// Processes the command.
        /// </summary>
        /// <param name="req">Command and Parameters</param>
        /// <returns>Result of ExecuteNonQuery</returns>
        int ProcessCommand(IRequest req);

        /// <summary>
        /// Processes the query.
        /// </summary>
        /// <param name="req">Command and Parameters</param>
        /// <returns>DataSet generated from query</returns>
        DataSet ProcessQuery(IRequest req);

        /// <summary>
        /// Processes the scalar query.
        /// </summary>
        /// <param name="req">Command and Parameters</param>
        /// <returns>Single value, first row and column value.</returns>
        object ProcessScalar(IRequest req);

        /// <summary>
        /// Creates a new command and parameter request.
        /// </summary>
        /// <returns>New command and parameter request object</returns>
        IRequest NewRequest();
    }

    /// <summary>
    /// Database query processor, is implementation to process, execute and return result for database query.
    /// </summary>
    public class DbQueryProcessor : IDbQueryProcessor
    {
        public class TableName
        {
            public Dictionary<string, string> Franchise { get; set; }

            public Dictionary<string, string> Base { get; set; }

            public Dictionary<string, string> Core { get; set; }
        }

        #region Private Members

        private IDbConnectionHelper _connectionHelper;


        #endregion Private Members

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryProcessor" /> class.
        /// </summary>
        /// <param name="connectionHelper">The connection helper.</param>
        public DbQueryProcessor(IDbConnectionHelper connectionHelper)
        {
            _connectionHelper = connectionHelper;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Processes the command.
        /// </summary>
        /// <param name="req">Command and Parameters</param>
        /// <returns>
        /// Result of ExecuteNonQuery
        /// </returns>
        public int ProcessCommand(IRequest req)
        {
            SqlCommand sql = null;
            int result = 0;

            try
            {
                SqlConnection connectionObject = _connectionHelper.Open() as SqlConnection;

                sql = new SqlCommand();
                sql.Connection = connectionObject;
                sql.CommandText = req.Commands;
                sql.CommandType = CommandType.StoredProcedure;

                if (req.RequestParameters != null)
                {
                    foreach (SqlParameter setProp in req.RequestParameters)
                    {
                        sql.Parameters.AddWithValue("@" + setProp.ParameterName, setProp.Value);
                    }
                }
                result = sql.ExecuteNonQuery();
            }
            finally
            {
                if (sql != null)
                {
                    sql.Dispose();
                    sql = null;
                }
            }

            return result;
        }

        /// <summary>
        /// Processes the query.
        /// </summary>
        /// <param name="req">Command and Parameters</param>
        /// <returns>
        /// DataSet generated from query
        /// </returns>
        public DataSet ProcessQuery(IRequest req)
        {
            SqlDataAdapter da = null;
            DataSet ds = null;
            SqlCommand sql = null;

            try
            {
                ds = new DataSet();
                SqlConnection connectionObject = _connectionHelper.Open() as SqlConnection;
                sql = new SqlCommand();
                sql.Connection = connectionObject;
                sql.CommandText = req.Commands;
                sql.CommandType = CommandType.StoredProcedure;

                if (req.RequestParameters != null)
                {
                    foreach (SqlParameter setProp in req.RequestParameters)
                    {
                        sql.Parameters.AddWithValue("@" + setProp.ParameterName, setProp.Value);
                    }
                }
                da = new SqlDataAdapter(sql);
                da.Fill(ds);

                return ds;
            }
            finally
            {
                if (da != null)
                {
                    da.Dispose();
                    da = null;
                }
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
                if (sql != null)
                {
                    sql.Dispose();
                    sql = null;
                }
            }
        }

        /// <summary>
        /// Processes the scalar query.
        /// </summary>
        /// <param name="req">Command and Parameters</param>
        /// <returns>
        /// Single value, first row and column value.
        /// </returns>
        public object ProcessScalar(IRequest req)
        {
            DataSet ds = ProcessQuery(req);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0][0];
            }
            return null;
        }


        /// <summary>
        /// Creates a new command and parameter request.
        /// </summary>
        /// <returns>
        /// New command and parameter request object
        /// </returns>
        public IRequest NewRequest()
        {
            return new Request();
        }

        #endregion Public Methods
    }
}
