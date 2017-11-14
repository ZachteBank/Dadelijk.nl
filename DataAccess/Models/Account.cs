using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Account
    {
        public Account()
        {
            Id = 0;
        }
        public Account(int id)
        {
            Id = id;
        }

        private string _passHash;
        public string PassHash { get => _passHash; set => _passHash = value ?? throw new ArgumentNullException("PassHash"); }

        private string _email;
        public string Email { get => _email; set => _email = value ?? throw new ArgumentNullException("Email"); }


        public bool EmailConfirmed;

        public int Id { get; private set; }
    }
}
