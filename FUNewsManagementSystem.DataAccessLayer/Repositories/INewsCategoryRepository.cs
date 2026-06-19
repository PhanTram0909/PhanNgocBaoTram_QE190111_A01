using FUNewsManagementSystem.Model;

namespace FUNewsManagementSystem.DataAccessLayer.Repositories
{
    public interface INewsCategoryRepository : IGenericRepository<NewsCategory>
    {
        Task<IEnumerable<NewsCategory>> GetAllWithParentAsync();
        Task<IEnumerable<NewsCategory>> SearchAsync(string? searchTerm);
        Task<bool> HasArticlesAsync(short categoryId);
    }
}
