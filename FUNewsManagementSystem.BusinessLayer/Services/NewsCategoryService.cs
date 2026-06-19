using FUNewsManagementSystem.DataAccessLayer.Repositories;
using FUNewsManagementSystem.Model;

namespace FUNewsManagementSystem.BusinessLayer.Services
{
    public class NewsCategoryService : INewsCategoryService
    {
        private readonly INewsCategoryRepository _categoryRepo;

        public NewsCategoryService(INewsCategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<NewsCategory>> GetAllCategoriesAsync()
            => await _categoryRepo.GetAllWithParentAsync();

        public async Task<IEnumerable<NewsCategory>> GetActiveCategoriesAsync()
        {
            var all = await _categoryRepo.GetAllWithParentAsync();
            return all.Where(c => c.IsActive);
        }

        public async Task<NewsCategory?> GetCategoryByIdAsync(short id)
            => await _categoryRepo.GetByIdAsync(id);

        public async Task<IEnumerable<NewsCategory>> SearchCategoriesAsync(string? searchTerm)
            => await _categoryRepo.SearchAsync(searchTerm);

        public async Task<bool> CreateCategoryAsync(NewsCategory category)
        {
            await _categoryRepo.AddAsync(category);
            return true;
        }

        public async Task<bool> UpdateCategoryAsync(NewsCategory category)
        {
            var existing = await _categoryRepo.GetByIdAsync(category.CategoryId);
            if (existing == null) return false;

            existing.CategoryName = category.CategoryName;
            existing.CategoryDescription = category.CategoryDescription;
            existing.ParentCategoryId = category.ParentCategoryId;
            existing.IsActive = category.IsActive;

            await _categoryRepo.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(short id)
        {
            if (!await CanDeleteCategoryAsync(id)) return false;
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return false;
            await _categoryRepo.DeleteAsync(category);
            return true;
        }

        public async Task<bool> CanDeleteCategoryAsync(short id)
            => !await _categoryRepo.HasArticlesAsync(id);
    }
}
