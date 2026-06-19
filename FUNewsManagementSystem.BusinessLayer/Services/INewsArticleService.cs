using FUNewsManagementSystem.Model;

namespace FUNewsManagementSystem.BusinessLayer.Services
{
    public interface INewsArticleService
    {
        Task<IEnumerable<NewsArticle>> GetAllArticlesAsync();
        Task<IEnumerable<NewsArticle>> GetActiveArticlesAsync();
        Task<NewsArticle?> GetArticleByIdAsync(string id);
        Task<IEnumerable<NewsArticle>> SearchArticlesAsync(string? searchTerm, bool? statusFilter = null);
        Task<IEnumerable<NewsArticle>> GetArticlesByCreatorAsync(short createdById);
        Task<IEnumerable<NewsArticle>> GetArticlesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> CreateArticleAsync(NewsArticle article, List<int> tagIds);
        Task<bool> UpdateArticleAsync(NewsArticle article, List<int> tagIds, short updatedById);
        Task<bool> DeleteArticleAsync(string id, short requestingUserId);
    }
}
