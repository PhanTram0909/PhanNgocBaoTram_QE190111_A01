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
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountName,AccountEmail,AccountRole,AccountPassword")] SystemAccount account)
        {
            ModelState.Remove("account.CreatedArticles");
            ModelState.Remove("account.UpdatedArticles");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validation failed: " + string.Join(", ", errors) });
            }

            if (await _accountService.EmailExistsAsync(account.AccountEmail))
                return Json(new { success = false, message = "Email already exists." });

            // Auto-assign ID
            var all = await _accountService.GetAllAccountsAsync();
            account.AccountId = all.Any() ? (short)(all.Max(a => a.AccountId) + 1) : (short)1;

            try
            {
                var result = await _accountService.CreateAccountAsync(account);
                return Json(new { success = result, message = result ? "Account created successfully." : "Failed to create account." });
            }
            catch (Exception ex)
            {
                // If the error indicates identity column issue, try saving without manual ID
                if (ex.Message.Contains("IDENTITY_INSERT") || (ex.InnerException?.Message.Contains("IDENTITY_INSERT") ?? false))
                {
                    try
                    {
                        // Reset ID to let SQL Server generate it automatically
                        account.AccountId = 0; 
                        var result = await _accountService.CreateAccountAsync(account);
                        return Json(new { success = result, message = result ? "Account created successfully." : "Failed to create account." });
                    }
                    catch (Exception ex2)
                    {
                        return Json(new { success = false, message = "Error: " + (ex2.InnerException?.Message ?? ex2.Message) });
                    }
                }
                return Json(new { success = false, message = "Error: " + (ex.InnerException?.Message ?? ex.Message) });
            }
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
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("AccountId,AccountName,AccountEmail,AccountRole,AccountPassword")] SystemAccount account)
        {
            if (string.IsNullOrWhiteSpace(account.AccountPassword))
            {
                ModelState.Remove("account.AccountPassword");
            }
            ModelState.Remove("account.CreatedArticles");
            ModelState.Remove("account.UpdatedArticles");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validation failed: " + string.Join(", ", errors) });
            }

            if (await _accountService.EmailExistsAsync(account.AccountEmail, account.AccountId))
                return Json(new { success = false, message = "Email already exists." });

            try
            {
                var result = await _accountService.UpdateAccountAsync(account);
                return Json(new { success = result, message = result ? "Account updated successfully." : "Account not found." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + (ex.InnerException?.Message ?? ex.Message) });
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(short id)
        {
            var currentId = HttpContext.Session.GetString("AccountId");
            if (currentId == id.ToString())
                return Json(new { success = false, message = "You cannot delete your own account." });

            try
            {
                var result = await _accountService.DeleteAccountAsync(id);
                return Json(new { success = result, message = result ? "Account deleted successfully." : "Account not found." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Cannot delete this account because it has created or updated news articles in the database." });
            }
        }

        // Admin statistics
        public async Task<IActionResult> Statistics()
        {
            return View();
        }
    }
}
