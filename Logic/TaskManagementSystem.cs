using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using DataAccess.Repositories;

namespace Logic
{
    class TaskManagementSystem
    {
        private AccountRespository _accountRespository;
        private NewsItemRepository _newsItemRepository;
        public TaskManagementSystem(string connectionString)
        {
            DatabaseSettings dbSettings = new DatabaseSettings();
            dbSettings.SetConnectionString(connectionString);

            _accountRespository = new AccountRespository(dbSettings);
            _newsItemRepository = new NewsItemRepository(dbSettings);
        }
    }
}
