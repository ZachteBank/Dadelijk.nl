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
        
        public string PassHash;
        public string Email;
        public bool EmailConfirmed;

        public int Id { get; private set; }
    }
}
