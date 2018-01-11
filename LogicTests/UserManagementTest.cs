using Dadelijk.nl;
using Logic;
using System;
using System.Linq;
using Models;
using Xunit;

namespace LogicTests
{
    public class UserManagementTest
    {
        private UserManagementSystem _ums;
        string email = "test@test.nl";


        public UserManagementTest()
        {
            _ums = new UserManagementSystem("Data Source=volunteersapp.c153q9deg6j1.us-east-1.rds.amazonaws.com;Initial Catalog=bram;User id=App_bF72Esbab9RD;Password=Gq96h8MhY6JckP9ESScs3SfD;");
        }

        [Fact]
        public void TestDeleteAccount()
        {
            var account = _ums.GetAccountByEmail(email);
            if (account != null)
            {
                _ums.Register(email, "testPassword", "testUsername");
                account = _ums.GetAccountByEmail(email);
            }

            if (account != null)
            {
                _ums.DeleteAccount(account.Id);
                account = _ums.GetAccountByEmail(email);
                Assert.Null(account);
            }
        }

        [Fact]
        public void TestRegisterAccount()
        {
            var account = _ums.GetAccountByEmail(email);
            if (account != null)
            {
                _ums.DeleteAccount(account.Id);
            }

            account = _ums.GetAccountByEmail(email);
            if (account != null)
            {
                Assert.True(false); //Register cant continue
            }

            _ums.Register(email, "testPassword", "testAccount");
            account = _ums.GetAccountByEmail(email);
            
            Assert.NotNull(account);
            Assert.Equal(account.Email, email);
            Assert.Equal(account.UserName, "testAccount");

            _ums.DeleteAccount(account.Id);


        }

        
    }
}
