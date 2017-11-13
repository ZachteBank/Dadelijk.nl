using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using DataAccess.Exceptions;

namespace DataAccess
{
    public class DatabaseSettings
    {
        private string _connectionString;

        /// <summary>
        /// Sets the connection string, throws an exception when the string is not valid
        /// </summary>
        /// <param name="connectionString"></param>
        /// <exception cref="InvalidConnectionString"></exception>
        public void SetConnectionString(string connectionString)
        {
            try
            {
                new SqlConnectionStringBuilder(connectionString);
            }
            catch (Exception)
            {
                throw new InvalidConnectionString();
            }


            _connectionString = connectionString;
        }

        /// <summary>
        /// Gives a valid connection string, throws an exception otherwise
        /// </summary>
        /// <returns>a valid connection string</returns>
        /// <exception cref="ConnectionStringNotSetException"></exception>
        public string GetConnectionString()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new ConnectionStringNotSetException();
            }
            return _connectionString;
        }

        public void ClearConnectionString()
        {
            _connectionString = "";
        }
    }
}
