﻿using System;
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
        private ReactionRepository _reactionRepository;

        public TaskManagementSystem(string connectionString)
        {
            DatabaseSettings dbSettings = new DatabaseSettings();
            dbSettings.SetConnectionString(connectionString);

            _accountRespository = new AccountRespository(dbSettings);
            _newsItemRepository = new NewsItemRepository(dbSettings);
            _reactionRepository = new ReactionRepository(dbSettings);
        }

        public bool CreateReaction(int accountId, int newsItemId, string text, int reactionId = 0)
        {
            var reaction = new Reaction()
            {
                Account = _accountRespository.GetAccountById(accountId),
                NewsItemId = newsItemId,
                ParentReaction = reactionId > 0 ? _reactionRepository.GetReactionById(reactionId) : null,
                Text = text,
            };

            return AddReaction(reaction);
        }

        public bool AddReaction(Reaction reaction)
        {
            return _reactionRepository.CreateReaction(reaction);
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
            return (onlyActive ? _newsItemRepository.GetAllNewsItems().Where(x => x.Active) : _newsItemRepository.GetAllNewsItems()).Take(25).OrderByDescending(x => x.DateUpdatedOrCreated);
        }

        public IEnumerable<NewsItem> AllNewsItems(DateTime date, bool onlyActive = true)
        {
            return (onlyActive ? _newsItemRepository.GetAllNewsItems(date).Where(x => x.Active) : _newsItemRepository.GetAllNewsItems(date)).OrderByDescending(x => x.DateUpdatedOrCreated);
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

        public IEnumerable<Reaction> GetAllReactionsFromNewsItemFormatted(NewsItem newsItem)
        {
            return GetAllReactionsFromNewsItemFormatted(newsItem.Id);
        }

        public IEnumerable<Reaction> GetAllReactionsFromNewsItemFormatted(int newsItemId)
        {
            var finalList = new List<Reaction>();

            List<Reaction> reactions = _reactionRepository.GetAllReactionsByNewsItemId(newsItemId).OrderBy(x => x.DateUpdatedOrCreated).ToList();
            while (reactions.Any())
            {
                var maxOffset = reactions.Max(x => x.GetOffset());
                var tmpReactions = reactions.Where(x => x.GetOffset() == maxOffset).ToList();
                for (int i = 0; i < tmpReactions.Count();i++)
                {
                    var reaction = tmpReactions[i];
                    reactions.Remove(reaction);
                    finalList.Add(reaction);

                    var tmpReaction = reaction;

                    while (tmpReaction.ParentReaction != null)
                    {
                        if (finalList.All(x => x.Id != tmpReaction.ParentReaction.Id))
                        {
                            reactions.RemoveAt(reactions.FindIndex(x => x.Id == tmpReaction.ParentReaction.Id));
                            finalList.Insert(finalList.IndexOf(tmpReaction), tmpReaction.ParentReaction);
                        }
                        tmpReaction = tmpReaction.ParentReaction;
                    }
                }
            }
            return finalList;
        }

        public void DeleteNewsItem(NewsItem newsItem)
        {
            _newsItemRepository.RemoveNewsItem(newsItem);
        }
    }
}
