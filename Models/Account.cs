using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Account : BaseModel
    {
        public Account(int id = 0) : base(id)
        {
        }

        private string _email;
        public string Email { get => _email; set => _email = value ?? throw new ArgumentNullException("Email"); }

        private string _passHash;
        public string PassHash { get => _passHash; set => _passHash = value ?? throw new ArgumentNullException("PassHash"); }
        
        public AccountType AccountType { get; set; }



    }
}
