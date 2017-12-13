using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Dadelijk.nl.Controllers
{
    public class CMSController : Controller
    {
        //CONNECTION STRING: Data Source=volunteersapp.c153q9deg6j1.us-east-1.rds.amazonaws.com;Initial Catalog=bram;User id=App_bF72Esbab9RD;Password=Gq96h8MhY6JckP9ESScs3SfD;
        private UserManagementSystem _ums = new UserManagementSystem(Startup.ConnectionString);
        private TaskManagementSystem _tms = new TaskManagementSystem(Startup.ConnectionString);
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                //Login
                Account account = null;
                try
                {
                    account = _ums.Login(email, password);
                }
                catch (Exception e)
                {
                    ViewBag.Error = "Er ging iets mis, probeer het opnieuw. Error: "+e.Message;
                }
                if (account == null)
                {
                    ViewBag.Error = "Er ging iets mis, probeer het opnieuw";
                }
                else
                {
                    HttpContext.Session.SetInt32("id", account.Id);
                    ViewBag.Success = "Inloggen gelukt!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                Account account = null;
                //Register
                try
                {
                    account = _ums.Register(email, password);
                }
                catch (Exception e)
                {
                    ViewBag.Error = "Er ging iets mis, probeer het opnieuw. Error: "+e.Message;
                }
                if (account == null)
                {
                    ViewBag.Error = "Er ging iets mis, probeer het opnieuw";
                }
                else
                {
                    HttpContext.Session.SetInt32("id", account.Id);
                    ViewBag.Success = "Registreren gelukt!";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNewsItem(string subject, string text, bool active)
        {
            if (_tms.CreateNewsItem(subject, text, active))
            {
                ViewBag.Success = "Nieuws item toegevoegd";
            }
            return View();
        }

        public IActionResult AddNewsItem()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangeStateNewsItem(int newsItemId)
        {
            var item = _tms.GetNewsItemById(newsItemId);
            item.Active = !item.Active;
            _tms.UpdateNewsItem(item);
            TempData["Success"] = "Nieuwsbericht is " + (item.Active ? "aangezet" : "uitgezet");
            return RedirectToAction("AllNewsItems");
        }

        public IActionResult AllNewsItems()
        {
            if(TempData["Success"] != null)
                ViewBag.Success = TempData["Success"].ToString();

            var newsItems = _tms.AllNewsItems(false);
            return View(newsItems);
        }

        [HttpGet]
        public IActionResult EditNewsItem(int newsItemId)
        {
            var newsItem = _tms.GetNewsItemById(newsItemId);
            return View(newsItem);
        }

        [HttpPost]
        public IActionResult EditNewsItem(int newsItemId, string subject, string text, bool active)
        {
            var newsItem = _tms.GetNewsItemById(newsItemId);

            newsItem.Subject = subject;
            newsItem.Text = text;
            newsItem.Active = active;

            _tms.UpdateNewsItem(newsItem);
            ViewBag.Success = "Update is gelukt";

            return View(newsItem);
        }



    }
}
