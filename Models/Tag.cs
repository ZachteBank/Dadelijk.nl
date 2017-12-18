using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Models
{
    public class Tag : BaseModel
    {
        public Tag(int id = 0) : base(id)
        {
        }

        public string Name { get; set; }
        
    }
}
