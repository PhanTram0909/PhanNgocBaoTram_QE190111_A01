using FUNewsManagementSystem.BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace FUManagementSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly ISystemAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AuthController(ISystemAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("AccountEmail") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required.";
                return View();
            }

            // Check admin account from config
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) && password == adminPassword)
            {
                HttpContext.Session.SetString("AccountEmail", email);
                HttpContext.Session.SetString("AccountName", "Administrator");
                HttpContext.Session.SetString("AccountRole", "0"); // Admin
                HttpContext.Session.SetString("AccountId", "0");
                return RedirectToAction("Index", "Accounts");
            }

            // Check DB accounts
            var account = await _accountService.LoginAsync(email, password);
            if (account != null)
            {
                HttpContext.Session.SetString("AccountEmail", account.AccountEmail);
                HttpContext.Session.SetString("AccountName", account.AccountName);
                HttpContext.Session.SetString("AccountRole", account.AccountRole.ToString());
                HttpContext.Session.SetString("AccountId", account.AccountId.ToString());

                return account.AccountRole switch
                {
                    1 => RedirectToAction("Index", "NewsArticles"), // Staff
                    2 => RedirectToAction("Index", "Home"),         // Lecturer
                    _ => RedirectToAction("Index", "Home")
                };
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
