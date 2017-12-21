using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Models
{
    public class NewsItem : BaseModel
    {
        public NewsItem(int id = 0) : base(id)
        {
            Reactions = new List<Reaction>();
            Tags = new List<Tag>();
        }

        public IEnumerable<Reaction> Reactions { get; set; }
        public IEnumerable<Tag> Tags { get; set; }


        public string Subject { get; set; }

        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                if(value == null) throw new ArgumentNullException();
                value = value.Replace("script", "niceTry");
                _text = value;
            }
        }
        private readonly Regex regex = new Regex("[^a-zA-Z0-9-]");
        public string SubjectUrl => regex.Replace(Subject, "-").Replace("--", "-");

        public bool Active { get; set; }

    }
}
