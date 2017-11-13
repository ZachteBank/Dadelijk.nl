using System;
using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;

namespace Logic
{
    public class UserManagementSystem
    {
        private AccountRespository _accountRespository;

        public UserManagementSystem(string connectionString)
        {
            DatabaseSettings dbSettings = new DatabaseSettings();
            dbSettings.SetConnectionString(connectionString);

            _accountRespository = new AccountRespository(dbSettings);
        }

        public Account Login(string email, string password)
        {
            return null;
        }

        private string HashPassword(string password)
        {
            //TODO: Make this method
            return password;
        }
    }
}
