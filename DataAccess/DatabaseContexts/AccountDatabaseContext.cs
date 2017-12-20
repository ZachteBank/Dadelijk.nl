﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using DataAccess.Repositories;
using Models;

namespace DataAccess.DatabaseContexts
{
    public class AccountDatabaseContext : BaseDatabaseContext, IAccountContext
    {
        public AccountDatabaseContext(DatabaseSettings settings) : base(settings)
        {
        }

        private Account CreateAccountByReader(SqlDataReader reader)
        {
            if (!reader.Read())
            {
                return null;
            }
            var account = new Account((int)reader["id"])
            {
                UserName = reader["username"].ToString(),
                Email = reader["email"].ToString(),
                PassHash = reader["passHash"].ToString(),
                AccountType = (AccountType)((reader["accountTypeId"] as int?) ?? 0),
            };

            AddDateCreatedAndDateUpdated(account, reader);

            return account;
        }

        public bool CreateAccount(Account account)
        {
            if (account?.Email == null || account.PassHash == null)
            {
                throw new ArgumentNullException();
            }

            if (account.Id != 0)
            {
                throw new ArgumentException("Account id should be 0");
            }

            if (account.Email.Length < 3)
            {
                throw new ArgumentException("Email is to short");
            }

            if (account.PassHash.Length < 3)
            {
                throw new ArgumentException("Pass is to short");
            }

            using (var connection = GetConnection())
            {

                var command = new SqlCommand(null, connection);
                command.CommandText =
                    @"INSERT INTO Account(email, passHash, accountTypeId, username) VALUES
                (@email, @password, @accountTypeId);
                SELECT SCOPE_IDENTITY() AS Id";


                command.Parameters.Add(new SqlParameter("email", account.Email));

                command.Parameters.Add(new SqlParameter("password", account.PassHash));

                command.Parameters.Add(new SqlParameter("accountTypeId", (int)account.AccountType));
                command.Parameters.Add(new SqlParameter("username", account.UserName));

                var reader = command.ExecuteReader();

                if (!reader.Read())
                {
                    return false;
                }
                SetIdOfModel(account, (int)reader.GetDecimal(0));
                return true;
            }
        }


        public Account GetAccountById(int id)
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(null, connection)
                {
                    CommandText = @"SELECT * FROM Account WHERE id=@id"
                };

                command.Parameters.Add(new SqlParameter("id", id));

                var reader = command.ExecuteReader();

                return CreateAccountByReader(reader);
            }
        }

        public Account GetAccountByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("email");
            }

            using (var conn = GetConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        @"SELECT * 
                    FROM dbo.Account
                    WHERE Account.email = @email";
                    cmd.Parameters.Add(new SqlParameter("email", email));

                    var reader = cmd.ExecuteReader();
                    return CreateAccountByReader(reader);
                }

            }
        }
    }
}
