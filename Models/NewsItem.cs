using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class NewsItem : BaseModel
    {
        public NewsItem(int id = 0) : base(id)
        {
        }
        
        public string Subject { get; set; }

        private string _text;

        public bool Active { get; set; }

        public string Text
        {
            get => _text;
            set
            {
                value = value.Replace("script", "niceTry");
                _text = value;
            }
        }
    }
}
