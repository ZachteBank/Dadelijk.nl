using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dadelijk.nl.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

    }
}
