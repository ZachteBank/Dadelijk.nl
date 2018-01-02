using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Dadelijk.nl.ViewModels
{
    public class NewsItemsOfDayAndRecent
    {
        public IEnumerable<NewsItem> OfDay { get; set; }

        public IEnumerable<NewsItem> Recent { get; set; }


    }
}
