using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Contexts;
using Models;

namespace DataAccess.Repositories
{
    public interface IAccountContext : IBaseContext
    {
        bool CreateAccount(Account account);

        Account GetAccountById(int id);

        Account GetAccountByEmail(string email);
    }
}
