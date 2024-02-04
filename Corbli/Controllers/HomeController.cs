using Corbli.Models;
using Corbli.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Diagnostics;
using System.Security.Claims;

namespace Corbli.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserDbService _userDbService;
        private PasswordHasher<LoginModel> _passwordHasher;

        public HomeController(
            ILogger<HomeController> logger,
            UserDbService userDbService,
            PasswordHasher<LoginModel> passwordHasher)
        {
            _logger = logger;
            _userDbService = userDbService;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult RegisterSubmit(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                string hash = _passwordHasher.HashPassword(loginModel, loginModel.Password);
                _userDbService.AddNewUser(loginModel.UserName, hash);
            }

            return new RedirectToActionResult("Index", "Home", null);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginSubmit(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                string user = loginModel.UserName;
                string hash = String.Empty;

                if (!_userDbService.QueryPasswordHash(user, ref hash))
                {
                    Debug.WriteLine("Login failed. Could not find user");
                }

                var checkRes = _passwordHasher.VerifyHashedPassword(
                    loginModel,
                    hash,
                    loginModel.Password);

                if (checkRes == PasswordVerificationResult.Success)
                {
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                    identity.AddClaim(new Claim("UserType", "Regular"));
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true });
                }
                else
                {
                    Debug.Print("Login failed - Invalid password");
                }
            }

            return new RedirectToActionResult("Index", "Home", null);
        }

        [Authorize(Policy = "IsLoggedIn")]
        public IActionResult Secret()
        {
            return View();
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
