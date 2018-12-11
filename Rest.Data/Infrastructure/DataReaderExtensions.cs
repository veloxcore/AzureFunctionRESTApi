using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Rest.Data.Infrastructure
{
    /// <summary>
    /// Extension methods for DataReader and DataRow
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Get Value from data reader as cast to <T>, if it is DBNull then return default(T)
        /// </summary>
        /// <typeparam name="T">Value to be converted to</typeparam>
        /// <param name="reader">Data reader from which to fetch value</param>
        /// <param name="columnName">Column's name from which to fetch value in data reader</param>
        /// <returns>Actual strongly type value, default(T) if column value is null</returns>
        public static T GetValueOrDefault<T>(this IDataReader reader, string columnName)
        {
            object columnValue = reader[columnName];
            T returnValue = default(T);
            if (!(columnValue is DBNull))
            {
                returnValue = (T)Convert.ChangeType(columnValue, typeof(T));
            }
            return returnValue;
        }

        /// <summary>
        /// Get Value from data reader as cast to <T>, if it is DBNull then return default(T)
        /// </summary>
        /// <typeparam name="T">Value to be converted to</typeparam>
        /// <param name="reader">Data reader from which to fetch value</param>
        /// <param name="columnId">Column's index from which to fetch value in data reader</param>
        /// <returns>Actual strongly type value, default(T) if column value is null</returns>
        public static T GetValueOrDefault<T>(this IDataReader reader, int columnId)
        {
            object columnValue = reader[columnId];
            T returnValue = default(T);
            if (!(columnValue is DBNull))
            {
                returnValue = (T)Convert.ChangeType(columnValue, typeof(T));
            }
            return returnValue;
        }

        /// <summary>
        /// Get Value from DataRow as cast to <T>, if it is DBNull then return default(T)
        /// </summary>
        /// <typeparam name="T">Value to be converted to</typeparam>
        /// <param name="dr">DataRow from which to fetch value</param>
        /// <param name="columnName">Column's name from which to fetch value in data row</param>
        /// <returns>Actual strongly type value, default(T) if column value is null</returns>
        public static T GetValueOrDefault<T>(this DataRow dr, string columnName)
        {
            object columnValue = dr[columnName];
            T returnValue = default(T);
            if (!(columnValue is DBNull))
            {
                returnValue = (T)Convert.ChangeType(columnValue, typeof(T));
            }
            return returnValue;
        }
    }
}
