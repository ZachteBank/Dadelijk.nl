using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Dadelijk.nl.ViewModels
{
    public class NewsItemAndReaction
    {
        public NewsItem NewsItem { get; set; }

        public IEnumerable<Reaction> Reactions { get; set; }
    }
}
