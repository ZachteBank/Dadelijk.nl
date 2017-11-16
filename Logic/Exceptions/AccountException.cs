using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Exceptions
{
    public class AccountException:Exception
    {
        public AccountException()
        {

        }
        public AccountException(string message) : base(message)
        {

        }
    }
}
