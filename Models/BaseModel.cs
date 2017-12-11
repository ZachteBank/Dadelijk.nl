using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public abstract class BaseModel
    {
        protected BaseModel(int id = 0)
        {
            Id = id;
        }

        public int Id { get; protected set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
