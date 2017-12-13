using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using DataAccess.Repositories;
using Models;

namespace Logic
{
    public class TaskManagementSystem
    {
        private AccountRespository _accountRespository;
        private NewsItemRepository _newsItemRepository;

        public TaskManagementSystem(string connectionString)
        {
            DatabaseSettings dbSettings = new DatabaseSettings();
            dbSettings.SetConnectionString(connectionString);

            _accountRespository = new AccountRespository(dbSettings);
            _newsItemRepository = new NewsItemRepository(dbSettings);
        }

        public bool CreateNewsItem(string subject, string text, bool active)
        {
            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException();
            }

            var newsItem = new NewsItem
            {
                Subject = subject,
                Text = text,
                Active = active
            };

            return AddNewsItem(newsItem);
        }

        public IEnumerable<NewsItem> AllNewsItems(bool onlyActive = true)
        {
            return onlyActive ? _newsItemRepository.GetAllNewsItems().Where(x => x.Active) : _newsItemRepository.GetAllNewsItems();
        }

        public IEnumerable<NewsItem> AllNewsItems(DateTime date, bool onlyActive = true)
        {
            return onlyActive ? _newsItemRepository.GetAllNewsItems(date).Where(x => x.Active) : _newsItemRepository.GetAllNewsItems(date);
        }

        public void EditNewsItem(NewsItem newsItem)
        {
            _newsItemRepository.UpdateNewsItem(newsItem);
        }

        public bool AddNewsItem(NewsItem newsItem)
        {
            return _newsItemRepository.CreateNewsItem(newsItem);
        }

        public NewsItem GetNewsItemById(int id)
        {
            return _newsItemRepository.GetNewsItemById(id);
        }

        public void UpdateNewsItem(NewsItem newsItem)
        {
            _newsItemRepository.UpdateNewsItem(newsItem);
        }
    }
}
