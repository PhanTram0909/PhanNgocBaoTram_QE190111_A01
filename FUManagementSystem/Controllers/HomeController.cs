using FUNewsManagementSystem.BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace FUManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsArticleService _articleService;

        public HomeController(INewsArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var role = HttpContext.Session.GetString("AccountRole");
            var articles = string.IsNullOrWhiteSpace(search)
                ? await _articleService.GetActiveArticlesAsync()
                : await _articleService.SearchArticlesAsync(search, statusFilter: true);

            ViewBag.Search = search;
            return View(articles);
        }

        public async Task<IActionResult> Details(string id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);
            if (article == null || !article.NewsStatus) return NotFound();
            return View(article);
        }
    }
}