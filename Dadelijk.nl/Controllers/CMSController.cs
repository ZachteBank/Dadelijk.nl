using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dadelijk.nl.Controllers
{
    public class CMSController : Controller
    {
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
