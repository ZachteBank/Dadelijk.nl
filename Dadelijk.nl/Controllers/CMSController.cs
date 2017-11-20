using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dadelijk.nl.Controllers
{
    public class CMSController : Controller
    {
        //CONNECTION STRING: Data Source=volunteersapp.c153q9deg6j1.us-east-1.rds.amazonaws.com;Initial Catalog=bram;User id=App_bF72Esbab9RD;Password=Gq96h8MhY6JckP9ESScs3SfD;
        private UserManagementSystem _ums = new UserManagementSystem(Startup.ConnectionString);
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
                }
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
