using FUManagementSystem.Filters;
using FUNewsManagementSystem.BusinessLayer.Services;
using FUNewsManagementSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace FUManagementSystem.Controllers
{
    [AuthorizeRole(1)] // Staff only
    public class NewsArticlesController : Controller
    {
        private readonly INewsArticleService _articleService;
        private readonly INewsCategoryService _categoryService;
        private readonly ITagService _tagService;

        public NewsArticlesController(
            INewsArticleService articleService,
            INewsCategoryService categoryService,
            ITagService tagService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var articles = await _articleService.SearchArticlesAsync(search);
            var categories = await _categoryService.GetActiveCategoriesAsync();
            var tags = await _tagService.GetAllTagsAsync();

            ViewBag.Search = search;
            ViewBag.Categories = categories;
            ViewBag.Tags = tags;
            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> GetArticle(string id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);
            if (article == null) return Json(null);
            return Json(new
            {
                article.NewsArticleId,
                article.NewsTitle,
                article.Headline,
                article.NewsContent,
                article.NewsSource,
                article.CategoryId,
                article.NewsStatus,
                TagIds = article.NewsTags?.Select(nt => nt.TagId).ToList() ?? new List<int>()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsArticle article, List<int> tagIds)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Validation failed." });

            var accountId = HttpContext.Session.GetString("AccountId");
            article.CreatedById = short.TryParse(accountId, out short id) ? id : (short)0;

            var result = await _articleService.CreateArticleAsync(article, tagIds ?? new List<int>());
            return Json(new { success = result, message = result ? "Article created successfully." : "Failed to create article." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsArticle article, List<int> tagIds)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Validation failed." });

            var accountId = HttpContext.Session.GetString("AccountId");
            short updatedById = short.TryParse(accountId, out short id) ? id : (short)0;

            var result = await _articleService.UpdateArticleAsync(article, tagIds ?? new List<int>(), updatedById);
            return Json(new { success = result, message = result ? "Article updated successfully." : "Failed to update article." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            short userId = short.TryParse(accountId, out short uid) ? uid : (short)0;

            var result = await _articleService.DeleteArticleAsync(id, userId);
            return Json(new { success = result, message = result ? "Article deleted." : "Cannot delete this article." });
        }

        public async Task<IActionResult> History()
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            if (!short.TryParse(accountId, out short id))
                return RedirectToAction("Login", "Auth");

            var articles = await _articleService.GetArticlesByCreatorAsync(id);
            return View(articles);
        }
    }
}
