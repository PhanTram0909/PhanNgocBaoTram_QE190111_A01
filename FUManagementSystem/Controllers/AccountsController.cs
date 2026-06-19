using FUManagementSystem.Filters;
using FUNewsManagementSystem.BusinessLayer.Services;
using FUNewsManagementSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace FUManagementSystem.Controllers
{
    [AuthorizeRole(0)] // Admin only
    public class AccountsController : Controller
    {
        private readonly ISystemAccountService _accountService;

        public AccountsController(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var accounts = await _accountService.SearchAccountsAsync(search);
            ViewBag.Search = search;
            return View(accounts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountName,AccountEmail,AccountRole,AccountPassword")] SystemAccount account)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Validation failed.", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            if (await _accountService.EmailExistsAsync(account.AccountEmail))
                return Json(new { success = false, message = "Email already exists." });

            // Auto-assign ID
            var all = await _accountService.GetAllAccountsAsync();
            account.AccountId = all.Any() ? (short)(all.Max(a => a.AccountId) + 1) : (short)1;

            var result = await _accountService.CreateAccountAsync(account);
            return Json(new { success = result, message = result ? "Account created successfully." : "Failed to create account." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAccount(short id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null) return Json(null);
            return Json(new
            {
                account.AccountId,
                account.AccountName,
                account.AccountEmail,
                account.AccountRole,
                account.AccountPassword
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("AccountId,AccountName,AccountEmail,AccountRole,AccountPassword")] SystemAccount account)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Validation failed." });

            if (await _accountService.EmailExistsAsync(account.AccountEmail, account.AccountId))
                return Json(new { success = false, message = "Email already exists." });

            var result = await _accountService.UpdateAccountAsync(account);
            return Json(new { success = result, message = result ? "Account updated successfully." : "Account not found." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(short id)
        {
            var currentId = HttpContext.Session.GetString("AccountId");
            if (currentId == id.ToString())
                return Json(new { success = false, message = "You cannot delete your own account." });

            var result = await _accountService.DeleteAccountAsync(id);
            return Json(new { success = result, message = result ? "Account deleted." : "Account not found." });
        }

        // Admin statistics
        public async Task<IActionResult> Statistics()
        {
            return View();
        }
    }
}
