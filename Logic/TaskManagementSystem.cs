using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using DataAccess.Repositories;

namespace Logic
{
    class TaskManagementSystem
    {
        public TaskManagementSystem(string connectionString)
        {
            DatabaseSettings dbSettings = new DatabaseSettings();
            dbSettings.SetConnectionString(connectionString);

            _accountRespository = new AccountRespository(dbSettings);
        }
    }
}
