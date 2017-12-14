using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Reaction : BaseModel
    {
        public Reaction(int id = 0) : base(id)
        {
        }
       public int NewsItemId { get; set; }
        public Account Account { get; set; }
        public Reaction ParentReaction { get; set; }

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                value = value.Replace("script", "niceTry");
                _text = value;
            }
        }
        public bool Active { get; set; }

        public int GetOffset(int max = 12)
        {
            if (ParentReaction == null)
            {
                return 0;
            }
            int i = 1;

            var parent = ParentReaction;

            while (parent.ParentReaction != null && i < max)
            {
                parent = parent.ParentReaction;
                i++;
            }
            return i;
        }
    }
}
