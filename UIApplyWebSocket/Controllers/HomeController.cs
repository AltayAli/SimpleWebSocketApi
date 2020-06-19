using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CheckUserInDatabase.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebSocketrealize.Models;

namespace WebSocketrealize.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext data;

        public HomeController(ILogger<HomeController> logger,
                                    DataContext context)
        {
            data = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(User user)
        {
            if (!data.Users.Any(x=>x.Email==user.Email))
            {
                data.Users.Add(user);
                data.SaveChanges();
            }
            else
            {
                user = data.Users.FirstOrDefault(x => x.Email == user.Email);
            }

           var d = data.Platforms.Where(x => x.UserId == user.Id).ToList();
            foreach(var item in d)
            {
                item.Status = 0;
            }
            data.SaveChanges();
            var p = new Platform
            {
                Value = Guid.NewGuid().ToString(),
                Status = 1,
                UserId = user.Id
            };
            data.Platforms.Add(p);
            data.SaveChanges();
            return RedirectToAction("Privacy",new {email = user.Email, platform = p.Value });
        }

        public IActionResult Privacy(string email, string platform)
        {
            ViewBag.Data = platform;
            ViewBag.Email = email;
            return View();
        }
       
    }
}
