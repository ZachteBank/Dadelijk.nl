using System;
using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;

namespace Logic
{
    public class UserManagementSystem
    {
        private readonly AccountRespository _accountRespository;

        public UserManagementSystem(string connectionString)
        {
            DatabaseSettings dbSettings = new DatabaseSettings();
            dbSettings.SetConnectionString(connectionString);

            _accountRespository = new AccountRespository(dbSettings);
        }

        public Account Login(string email, string password)
        {
            var account = _accountRespository.GetAccountByEmail(email);
            if (account != null && BCrypt.Net.BCrypt.Verify(password, account.PassHash))
            {
                return account;
            }
            return null;
        }

        public Account Register(string email, string password)
        {
            var account = new Account
            {
                Email = email,
                PassHash = HashPassword(password)
            };

            return _accountRespository.CreateAccount(account) ? account : null;
        }

        private string HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }
    }
}
