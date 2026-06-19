using FUManagementSystem.Filters;
using FUNewsManagementSystem.BusinessLayer.Services;
using FUNewsManagementSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace FUManagementSystem.Controllers
{
    [AuthorizeRole(0)] // Admin only
    public class ReportsController : Controller
    {
        private readonly INewsArticleService _articleService;

        public ReportsController(INewsArticleService articleService)
        {
            _articleService = articleService;
        }

        public IActionResult Statistics()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetStatistics(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                return Json(new { success = false, message = "Start date cannot be after end date." });

            var articles = await _articleService.GetArticlesByDateRangeAsync(startDate, endDate);
            var result = articles.Select(a => new
            {
                a.NewsArticleId,
                a.NewsTitle,
                a.Headline,
                CategoryName = a.Category?.CategoryName ?? "N/A",
                CreatedBy = a.CreatedBy?.AccountName ?? "N/A",
                a.CreatedDate,
                Status = a.NewsStatus ? "Active" : "Inactive"
            });

            return Json(new { success = true, data = result, count = articles.Count() });
        }
    }
}
