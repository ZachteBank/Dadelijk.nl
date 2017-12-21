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
            var dateCreated = reader["dateCreated"].ToString();
            baseModel.DateCreated = Convert.ToDateTime(dateCreated);
            baseModel.DateUpdated = !Convert.IsDBNull(reader["dateUpdated"])
                ? Convert.ToDateTime(reader["dateUpdated"].ToString())
                : (DateTime?) null;
        }

        protected void SetIdOfModel(object model, int id)
        {
            SetPropertyOfModel(model, id, "Id");
        }

        protected void SetDateUpdatedOfModel(object model, DateTime dateTime)
        {
            SetPropertyOfModel(model, dateTime, "DateUpdated");
        }

        private void SetPropertyOfModel(object model, object value, string property)
        {
            var t = model.GetType();
            if (t.GetProperty(property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
                throw new ArgumentOutOfRangeException(nameof(model), string.Format("Property {0} was not found in Type {1}", property, model.GetType().FullName));
            t.InvokeMember(property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.FlattenHierarchy, null, model, new object[] { value });
            /*var t = model.GetType();
            if (t.GetProperty("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
                throw new ArgumentOutOfRangeException(nameof(model), string.Format("Property {0} was not found in Type {1}", "Id", model.GetType().FullName));
            t.InvokeMember("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.FlattenHierarchy, null, model, new object[] { id });*/
        }
    }
}
