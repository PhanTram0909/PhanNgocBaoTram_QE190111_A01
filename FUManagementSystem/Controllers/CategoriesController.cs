using FUManagementSystem.Filters;
using FUNewsManagementSystem.BusinessLayer.Services;
using FUNewsManagementSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace FUManagementSystem.Controllers
{
    [AuthorizeRole(1)] // Staff only
    public class CategoriesController : Controller
    {
        private readonly INewsCategoryService _categoryService;

        public CategoriesController(INewsCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var categories = await _categoryService.SearchCategoriesAsync(search);
            var allCategories = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Search = search;
            ViewBag.AllCategories = allCategories;
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategory(short id)
        {
            var cat = await _categoryService.GetCategoryByIdAsync(id);
            if (cat == null) return Json(null);
            return Json(new
            {
                cat.CategoryId,
                cat.CategoryName,
                cat.CategoryDescription,
                cat.ParentCategoryId,
                cat.IsActive
            });
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryName,CategoryDescription,ParentCategoryId,IsActive")] NewsCategory category)
        {
            ModelState.Remove("category.ParentCategory");
            ModelState.Remove("category.SubCategories");
            ModelState.Remove("category.NewsArticles");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validation failed: " + string.Join(", ", errors) });
            }

            var result = await _categoryService.CreateCategoryAsync(category);
            return Json(new { success = result, message = result ? "Category created." : "Failed." });
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("CategoryId,CategoryName,CategoryDescription,ParentCategoryId,IsActive")] NewsCategory category)
        {
            ModelState.Remove("category.ParentCategory");
            ModelState.Remove("category.SubCategories");
            ModelState.Remove("category.NewsArticles");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validation failed: " + string.Join(", ", errors) });
            }

            var result = await _categoryService.UpdateCategoryAsync(category);
            return Json(new { success = result, message = result ? "Category updated." : "Category not found." });
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(short id)
        {
            try
            {
                var canDelete = await _categoryService.CanDeleteCategoryAsync(id);
                if (!canDelete)
                    return Json(new { success = false, message = "Cannot delete: category has assigned articles or subcategories." });

                var result = await _categoryService.DeleteCategoryAsync(id);
                return Json(new { success = result, message = result ? "Category deleted." : "Category not found." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Cannot delete this category due to database constraints." });
            }
        }
    }
}
