using System;

namespace Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
    }
}
