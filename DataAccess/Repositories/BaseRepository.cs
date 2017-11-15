﻿using System;
using System.Data.SqlClient;
using System.Reflection;

namespace DataAccess.Repositories
{
    public abstract class BaseRepository
    {
        protected DatabaseSettings Settings;

        protected BaseRepository(DatabaseSettings settings)
        {
            Settings = settings;
        }
        /// <summary>
        /// Gives a "new" connection to the database, please mind that "SqlConnection" uses connection pools
        /// </summary>
        /// <returns></returns>
        protected SqlConnection GetConnection()
        {
            var connection = new SqlConnection(Settings.GetConnectionString());
            connection.Open();
            return connection;
        }

        protected void SetIdOfModel(object model, int id)
        {
            var t = model.GetType();
            if (t.GetProperty("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
                throw new ArgumentOutOfRangeException(nameof(model), string.Format("Property {0} was not found in Type {1}", "Id", model.GetType().FullName));
            t.InvokeMember("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, model, new object[] { id });
            

        }
    }
}