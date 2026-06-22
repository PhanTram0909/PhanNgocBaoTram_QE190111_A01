using FUNewsManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.DataAccessLayer.Repositories
{
    public class NewsArticleRepository : GenericRepository<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(FUNewsManagementContext context) : base(context) { }

        public async Task<IEnumerable<NewsArticle>> GetAllWithDetailsAsync()
            => await _dbSet
                .AsNoTracking()
                .Include(a => a.Category)
                .Include(a => a.CreatedBy)
                .Include(a => a.NewsTags!)
                    .ThenInclude(nt => nt.Tag)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();

        public async Task<NewsArticle?> GetByIdWithDetailsAsync(string id)
            => await _dbSet
                .Include(a => a.Category)
                .Include(a => a.CreatedBy)
                .Include(a => a.UpdatedBy)
                .Include(a => a.NewsTags!)
                    .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(a => a.NewsArticleId == id);

        public async Task<IEnumerable<NewsArticle>> GetActiveArticlesAsync()
            => await _dbSet
                .AsNoTracking()
                .Where(a => a.NewsStatus)
                .Include(a => a.Category)
                .Include(a => a.CreatedBy)
                .Include(a => a.NewsTags!)
                    .ThenInclude(nt => nt.Tag)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();

        public async Task<IEnumerable<NewsArticle>> SearchAsync(string? searchTerm, bool? statusFilter = null)
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(a => a.Category)
                .Include(a => a.CreatedBy)
                .AsQueryable();

            if (statusFilter.HasValue)
                query = query.Where(a => a.NewsStatus == statusFilter.Value);

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(a => a.NewsTitle.Contains(searchTerm) ||
                                         a.Headline.Contains(searchTerm) ||
                                         (a.NewsContent != null && a.NewsContent.Contains(searchTerm)));

            return await query.OrderByDescending(a => a.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetByCreatorAsync(short createdById)
            => await _dbSet
                .AsNoTracking()
                .Where(a => a.CreatedById == createdById)
                .Include(a => a.Category)
                .Include(a => a.NewsTags!)
                    .ThenInclude(nt => nt.Tag)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();

        public async Task<IEnumerable<NewsArticle>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
            => await _dbSet
                .AsNoTracking()
                .Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate)
                .Include(a => a.Category)
                .Include(a => a.CreatedBy)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
    }
}
