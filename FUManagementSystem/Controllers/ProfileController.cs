using FUManagementSystem.Filters;
using FUNewsManagementSystem.BusinessLayer.Services;
using FUNewsManagementSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace FUManagementSystem.Controllers
{
    [AuthorizeRole] // Any authenticated user
    public class ProfileController : Controller
    {
        private readonly ISystemAccountService _accountService;

        public ProfileController(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            var role = HttpContext.Session.GetString("AccountRole");

            // Admin (role=0, id=0) - profile from config
            if (role == "0")
            {
                var adminProfile = new SystemAccount
                {
                    AccountId = 0,
                    AccountName = "Administrator",
                    AccountEmail = HttpContext.Session.GetString("AccountEmail") ?? "",
                    AccountRole = 0
                };
                return View(adminProfile);
            }

            if (!short.TryParse(accountId, out short id))
                return RedirectToAction("Login", "Auth");

            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null) return RedirectToAction("Login", "Auth");
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind("AccountId,AccountName,AccountEmail,AccountPassword")] SystemAccount account)
        {
            var role = HttpContext.Session.GetString("AccountRole");
            if (role == "0") // Admin cannot update profile here
                return Json(new { success = false, message = "Admin profile is managed via configuration." });

            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Validation failed." });

            var result = await _accountService.UpdateAccountAsync(account);
            if (result)
            {
                HttpContext.Session.SetString("AccountName", account.AccountName);
                HttpContext.Session.SetString("AccountEmail", account.AccountEmail);
            }
            return Json(new { success = result, message = result ? "Profile updated." : "Update failed." });
        }
    }
}
