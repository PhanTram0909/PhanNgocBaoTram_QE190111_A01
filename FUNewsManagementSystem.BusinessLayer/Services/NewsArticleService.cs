using FUNewsManagementSystem.DataAccessLayer;
using FUNewsManagementSystem.DataAccessLayer.Repositories;
using FUNewsManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.BusinessLayer.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _articleRepo;
        private readonly FUNewsManagementContext _context;

        public NewsArticleService(INewsArticleRepository articleRepo, FUNewsManagementContext context)
        {
            _articleRepo = articleRepo;
            _context = context;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllArticlesAsync()
            => await _articleRepo.GetAllWithDetailsAsync();

        public async Task<IEnumerable<NewsArticle>> GetActiveArticlesAsync()
            => await _articleRepo.GetActiveArticlesAsync();

        public async Task<NewsArticle?> GetArticleByIdAsync(string id)
            => await _articleRepo.GetByIdWithDetailsAsync(id);

        public async Task<IEnumerable<NewsArticle>> SearchArticlesAsync(string? searchTerm, bool? statusFilter = null)
            => await _articleRepo.SearchAsync(searchTerm, statusFilter);

        public async Task<IEnumerable<NewsArticle>> GetArticlesByCreatorAsync(short createdById)
            => await _articleRepo.GetByCreatorAsync(createdById);

        public async Task<IEnumerable<NewsArticle>> GetArticlesByDateRangeAsync(DateTime startDate, DateTime endDate)
            => await _articleRepo.GetByDateRangeAsync(startDate, endDate.AddDays(1).AddSeconds(-1));

        public async Task<bool> CreateArticleAsync(NewsArticle article, List<int> tagIds)
        {
            article.CreatedDate = DateTime.Now;
            article.ModifiedDate = DateTime.Now;

            // Generate unique ID
            if (string.IsNullOrWhiteSpace(article.NewsArticleId))
                article.NewsArticleId = "ART" + DateTime.Now.ToString("yyyyMMddHHmmss");

            await _articleRepo.AddAsync(article);

            // Add tags
            foreach (var tagId in tagIds)
            {
                _context.NewsTags.Add(new NewsTag
                {
                    NewsArticleId = article.NewsArticleId,
                    TagId = tagId
                });
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateArticleAsync(NewsArticle article, List<int> tagIds, short updatedById)
        {
            var existing = await _articleRepo.GetByIdWithDetailsAsync(article.NewsArticleId);
            if (existing == null) return false;

            existing.NewsTitle = article.NewsTitle;
            existing.Headline = article.Headline;
            existing.NewsContent = article.NewsContent;
            existing.NewsSource = article.NewsSource;
            existing.CategoryId = article.CategoryId;
            existing.NewsStatus = article.NewsStatus;
            existing.UpdatedById = updatedById;
            existing.ModifiedDate = DateTime.Now;

            await _articleRepo.UpdateAsync(existing);

            // Update tags: remove old, add new
            var oldTags = _context.NewsTags.Where(nt => nt.NewsArticleId == article.NewsArticleId);
            _context.NewsTags.RemoveRange(oldTags);
            foreach (var tagId in tagIds)
            {
                _context.NewsTags.Add(new NewsTag
                {
                    NewsArticleId = article.NewsArticleId,
                    TagId = tagId
                });
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteArticleAsync(string id, short requestingUserId)
        {
            var article = await _articleRepo.GetByIdWithDetailsAsync(id);
            if (article == null) return false;

            // Staff can only delete their own articles
            if (article.CreatedById != requestingUserId) return false;

            // Remove tags first
            var tags = _context.NewsTags.Where(nt => nt.NewsArticleId == id);
            _context.NewsTags.RemoveRange(tags);
            await _context.SaveChangesAsync();

            await _articleRepo.DeleteAsync(article);
            return true;
        }
    }
}
