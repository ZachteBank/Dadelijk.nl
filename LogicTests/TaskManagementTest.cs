using Dadelijk.nl;
using Logic;
using System;
using System.Linq;
using Models;
using Xunit;

namespace LogicTests
{
    public class TaskManagementTest
    {
        private TaskManagementSystem _tms;

        public TaskManagementTest()
        {
            _tms = new TaskManagementSystem("Data Source=volunteersapp.c153q9deg6j1.us-east-1.rds.amazonaws.com;Initial Catalog=bram;User id=App_bF72Esbab9RD;Password=Gq96h8MhY6JckP9ESScs3SfD;");
        }

        [Fact]
        public void AddNewsItem()
        {
            var newsItemsCount = _tms.AllNewsItems(false).Count();
            var newsItem = _addNewsItem();
            var newCount = _tms.AllNewsItems(false).Count();
            Assert.True(newCount > newsItemsCount);
            Assert.True(newsItem.Id != 0);

            Assert.Equal(newsItem.Active, false);
            Assert.Equal(newsItem.Subject, "XUnitTest");
            Assert.Equal(newsItem.Text, "Dit is een test bericht");
        }

        [Fact]
        private void AddNewsItemWithSubjectNull()
        {
            var newsItem = new NewsItem(){Subject = null, Text = "wauw", Active = false};
            Assert.Throws<ArgumentNullException>(() => _tms.AddNewsItem(newsItem));
        }
        [Fact]
        private void AddNewsItemWithTextNull()
        {
            Assert.Throws<ArgumentNullException>(() => new NewsItem() { Subject = "wauw", Text = null, Active = false });
        }
        [Fact]
        private void AddNewsItemWithTextNotSet()
        {
            var newsItem = new NewsItem() {Subject = "wauw", Text = "toch een text", Active = false};
            Assert.Throws<ArgumentNullException>(() => _tms.AddNewsItem(newsItem));
        }
        [Fact]
        private void AddNewsItemWithId()
        {
            var newsItem = new NewsItem(5) { Subject = "wauw", Text = "wauw", Active = false };
            Assert.Throws<ArgumentException>(() => _tms.AddNewsItem(newsItem));
        }

        [Fact]
        public void RemoveNewsItem()
        {
            var newsItem = _addNewsItem();
            var newsItemsCount = _tms.AllNewsItems(false).Count();
            _tms.DeleteNewsItem(newsItem);
            Assert.True(_tms.AllNewsItems(false).Count() < newsItemsCount);
        }

        private NewsItem _addNewsItem()
        {
            var newsItem = new NewsItem() { Subject = "XUnitTest", Text = "Dit is een test bericht", Active = false };
            _tms.AddNewsItem(newsItem);
            return newsItem;
        }
    }
}
