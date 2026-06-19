using FUNewsManagementSystem.Model;

namespace FUNewsManagementSystem.DataAccessLayer.Repositories
{
    public interface INewsArticleRepository : IGenericRepository<NewsArticle>
    {
        Task<IEnumerable<NewsArticle>> GetAllWithDetailsAsync();
        Task<NewsArticle?> GetByIdWithDetailsAsync(string id);
        Task<IEnumerable<NewsArticle>> GetActiveArticlesAsync();
        Task<IEnumerable<NewsArticle>> SearchAsync(string? searchTerm, bool? statusFilter = null);
        Task<IEnumerable<NewsArticle>> GetByCreatorAsync(short createdById);
        Task<IEnumerable<NewsArticle>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
