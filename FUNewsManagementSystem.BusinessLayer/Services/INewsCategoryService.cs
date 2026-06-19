using FUNewsManagementSystem.Model;

namespace FUNewsManagementSystem.BusinessLayer.Services
{
    public interface INewsCategoryService
    {
        Task<IEnumerable<NewsCategory>> GetAllCategoriesAsync();
        Task<IEnumerable<NewsCategory>> GetActiveCategoriesAsync();
        Task<NewsCategory?> GetCategoryByIdAsync(short id);
        Task<IEnumerable<NewsCategory>> SearchCategoriesAsync(string? searchTerm);
        Task<bool> CreateCategoryAsync(NewsCategory category);
        Task<bool> UpdateCategoryAsync(NewsCategory category);
        Task<bool> DeleteCategoryAsync(short id);
        Task<bool> CanDeleteCategoryAsync(short id);
    }
}
