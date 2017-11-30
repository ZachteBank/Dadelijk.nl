﻿using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using DataAccess.Repositories;
using Models;

namespace Logic
{
    class TaskManagementSystem
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

        public bool CreateNewsItem(string subject, string text)
        {
            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException();
            }

            var newsItem = new NewsItem
            {
                Subject = subject,
                Text = text
            };

            return AddNewsItem(newsItem);
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
