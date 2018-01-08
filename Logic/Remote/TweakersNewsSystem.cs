using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using DataAccess;
using DataAccess.Exceptions;
using DataAccess.Repositories;
using Logic.Json;
using Logic.Models;
using Models;
using Newtonsoft.Json;

namespace Logic
{
    public class TweakersNewsSystem: Remote.IBase
    {
        private string _url = "https://tweakers.net/feeds/nieuws.json";
        private string json;

        public TweakersNewsSystem()
        {
            json = new WebClient().DownloadString(_url);

        }

        public IEnumerable<IBaseModel> GetAllItems(int limit = 25)
        {
            var newsItemsJson = JsonConvert.DeserializeObject<List<TweakersJsonModel>>(json);
            var newsItems = new List<TweakersModel>();
            foreach (var newsItemJson in newsItemsJson)
            {
                var newsItem = new TweakersModel
                {
                    Title = newsItemJson.title,
                    Url = newsItemJson.link,
                    DateTime = UnixTimeStampToDateTime(newsItemJson.timestamp)
                };
                newsItems.Add(newsItem);
            }
            return newsItems.OrderByDescending(x => x.DateTime);

        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }
    }
}
