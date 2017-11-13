using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class AccountRespository : BaseRepository
    {
        private Account CreateAccountByReader(SqlDataReader reader)
        {
            if (!reader.Read())
            {
                return null;
            }
            return new Account((int)reader["id"])
            {
                Email = reader["email"].ToString(),
                EmailConfirmed = (bool)reader["EmailConfirmed"],
                PassHash = reader["passHash"].ToString()
            };
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
                    @"INSERT INTO Account(email, passHash, emailConfirmed) VALUES
                (@email, @password, @confirmed);
                SELECT SCOPE_IDENTITY() AS Id";


                command.Parameters.Add(new SqlParameter("email", account.Email));
                
                command.Parameters.Add(new SqlParameter("password", account.PassHash));

                command.Parameters.Add(new SqlParameter("confirmed", account.EmailConfirmed));

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

                var command = new SqlCommand(null, connection);
                command.CommandText =
                    @"SELECT * FROM account WHERE id=@id";


                command.Parameters.Add(new SqlParameter("id", id));

                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }

                var account = new Account((int)reader["id"])
                {
                    Email = (string)reader["email"],
                    EmailConfirmed = (bool)reader["emailConfirmed"],
                    PassHash = (string)reader["passHash"]
                };

                return account;
            }
        }

        public AccountRespository(DatabaseSettings settings) : base(settings)
        {
        }
    }
}
