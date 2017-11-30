using System;
using DataAccess;
using DataAccess.Exceptions;
using DataAccess.Repositories;
using Models;

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

        public Account GetAccountByEmail(string email)
        {
            return _accountRespository.GetAccountByEmail(email);
        }
        public Account GetAccountById(int id)
        {
            return _accountRespository.GetAccountById(id);
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
            if (GetAccountByEmail(email) != null)
            {
                throw new AccountException("Email already exists!");
            }
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
