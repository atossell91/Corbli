using Corbli.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Diagnostics;

namespace Corbli.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MySqlDataSource _dataSource;

        public HomeController(ILogger<HomeController> logger, MySqlDataSource dataSrc)
        {
            _logger = logger;
            _dataSource = dataSrc;
        }

        public IActionResult Index()
        {
            var con = _dataSource.OpenConnection();

            string qryStr = 
                "SELECT User.ID, User.Username, RoleInfo.RoleDescription " +
                "FROM USERINFO AS User " +
                "JOIN USERROLES AS UserRole ON User.ID = UserRole.UserID " +
                "JOIN ROLES AS RoleInfo ON UserRole.RoleID = RoleInfo.ID;";

            var qry = new MySqlCommand(qryStr, con);
            var res = qry.ExecuteReader();

            List<UserRole> roles = new List<UserRole>();
            while (res.Read())
            {
                roles.Add(new UserRole() {
                    Id = res.GetInt32(0),
                    Name = res.GetString(1),
                    Role = res.GetString(2)
                });
            }

            return View(roles);
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
