using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    class NewsItem : BaseModel
    {
        public NewsItem(int id = 0) : base(id)
        {
        }
        
        public string Subject { get; set; }

        public string Text { get; set; }
    }
}
