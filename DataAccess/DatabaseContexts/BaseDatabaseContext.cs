using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Models;

namespace DataAccess.DatabaseContexts
{
    public class BaseDatabaseContext
    {
        protected DatabaseSettings Settings;

        protected BaseDatabaseContext(DatabaseSettings settings)
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
                : (DateTime?)null;
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
                throw new ArgumentOutOfRangeException(nameof(model),
                    string.Format("Property {0} was not found in Type {1}", property, model.GetType().FullName));
            t.InvokeMember(property,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance |
                BindingFlags.FlattenHierarchy, null, model, new object[] {value});
        }
    }
}
