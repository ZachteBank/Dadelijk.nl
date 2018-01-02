using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dadelijk.nl.ViewModels;
using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Dadelijk.nl.Controllers
{
    public class GeneralController : Controller
    {
        //CONNECTION STRING: Data Source=volunteersapp.c153q9deg6j1.us-east-1.rds.amazonaws.com;Initial Catalog=bram;User id=App_bF72Esbab9RD;Password=Gq96h8MhY6JckP9ESScs3SfD;
        private UserManagementSystem _ums = new UserManagementSystem(Startup.ConnectionString);
        private TaskManagementSystem _tms = new TaskManagementSystem(Startup.ConnectionString);

        public IActionResult Index()
        {
            return RedirectToAction("AllNewsItems");
        }

        [Route("nieuws-berichten/{year?}/{month?}/{day?}")]
        public IActionResult AllNewsItems(int year, int month, int day)
        {
            DateTime dateTime;
            dateTime = year == 0 ? DateTime.Today : new DateTime(year, month, day);

            ViewBag.Date = dateTime.Ticks;
            var newsItems = new NewsItemsOfDayAndRecent
            {
                OfDay = _tms.AllNewsItems(dateTime),
                Recent = _tms.AllNewsItems()
            };
            return View(newsItems);
        }

        [Route("/artikel/{id:int}/{*subject}")]
        public IActionResult NewsItem(int id)
        {
            var newsItem = _tms.GetNewsItemById(id);
            if (newsItem == null)
            {
                return RedirectToAction("Index");
            }
            if (TempData["Success"] != null)
                ViewBag.Success = TempData["Success"].ToString();
            if (TempData["Error"] != null)
                ViewBag.Error = TempData["Error"].ToString();

            var newsItemAndReactions = new NewsItemAndReaction()
            {
                NewsItem = newsItem,
                Reactions = _tms.GetAllReactionsFromNewsItemFormatted(newsItem.Id)
            };
            return View(newsItemAndReactions);
        }

        [HttpPost]
        public IActionResult AddReaction(int newsItemId, string text, int reactionId = 0)
        {
            var newsItem = _tms.GetNewsItemById(newsItemId);
            if (isLoggedIn())
            {
                _tms.CreateReaction((int) HttpContext.Session.GetInt32("id"), newsItemId, text, reactionId);
                TempData["Success"] = "Reactie is gepost";

            }
            else
            {
                TempData["Error"] = "Niet ingelogd";
            }
            return RedirectToAction("NewsItem", "General", new {id = newsItemId, subject=newsItem.SubjectUrl});
        }

        private bool isLoggedIn()
        {
            return HttpContext.Session.GetInt32("id") != null;
        }


    }
}
