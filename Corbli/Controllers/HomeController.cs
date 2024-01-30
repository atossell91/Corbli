using Corbli.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Diagnostics;

namespace Corbli.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MySqlDataSource dataSrc)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginSubmit(LoginModel model)
        {
            if (ModelState.IsValid)
            {
            }

            return new RedirectToActionResult("Index", "Home", null);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
