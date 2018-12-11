using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Rest.Data.Infrastructure
{
    /// <summary>
    /// DataBase connection helper implementation, it will provide correct franchise connection based on the logged in user.
    /// NOTE: Don't use it for initial loading as at that time we don't have logged in user nor current company details.
    /// </summary>
    public interface IDbConnectionHelper
    {
        /// <summary>
        /// Closes this instance.
        /// </summary>
        void Close();

        /// <summary>
        /// Creates a new connection if not already exists, and opens it.
        /// </summary>
        /// <returns>Open database connection.</returns>
        IDbConnection Open();

        /// <summary>
        /// Gets the get current connection.
        /// </summary>
        /// <value>
        /// The get current connection.
        /// </value>
        IDbConnection CurrentConnection { get; }
    }

    /// <summary>
    /// DataBase connection helper implementation, it will provide correct franchise connection based on the logged in user.
    /// NOTE: Don't use it for initial loading as at that time we don't have logged in user nor current company details.
    /// </summary>
    public class DbConnectionHelper : IDisposable, IDbConnectionHelper
    {
        #region Fields

        private string _connectionString;

        private IDbConnection _connection;

        private IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(_connectionString);
                }
                return _connection;
            }
        }

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionHelper" /> class.
        /// </summary>
        public DbConnectionHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion Constructor

        #region Properties

        public IDbConnection CurrentConnection => Connection;

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (Connection != null && Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
        }

        /// <summary>
        /// Creates a new connection if not already exists, and opens it.
        /// </summary>
        /// <returns>Open database connection.</returns>
        public IDbConnection Open()
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
            return Connection;

        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        #endregion Public Methods

    }
}
