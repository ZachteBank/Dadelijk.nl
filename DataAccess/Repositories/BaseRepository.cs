using System;
using System.Data.SqlClient;
using System.Reflection;
using Models;

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

        protected void AddDateCreatedAndDateUpdated(BaseModel baseModel, SqlDataReader reader)
        {
            baseModel.DateCreated = Convert.ToDateTime(reader["dateCreated"].ToString());
            baseModel.DateUpdated = !Convert.IsDBNull(reader["dateUpdated"])
                ? Convert.ToDateTime(reader["dateUpdated"].ToString())
                : (DateTime?) null;
        }

        protected void SetIdOfModel(object model, int id)
        {
            var t = model.GetType();
            if (t.GetProperty("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
                throw new ArgumentOutOfRangeException(nameof(model), string.Format("Property {0} was not found in Type {1}", "Id", model.GetType().FullName));
            t.InvokeMember("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.FlattenHierarchy, null, model, new object[] { id });
        }

        protected void SetDateUpdatedOfModel(object model, DateTime dateTime)
        {
            var t = model.GetType();
            if (t.GetProperty("DateUpdated", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
                throw new ArgumentOutOfRangeException(nameof(model), string.Format("Property {0} was not found in Type {1}", "DateUpdated", model.GetType().FullName));
            t.InvokeMember("DateUpdated", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.FlattenHierarchy, null, model, new object[] { dateTime });
        }
    }
}
