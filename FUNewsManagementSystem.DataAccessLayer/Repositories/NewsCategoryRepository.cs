using FUNewsManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.DataAccessLayer.Repositories
{
    public class NewsCategoryRepository : GenericRepository<NewsCategory>, INewsCategoryRepository
    {
        public NewsCategoryRepository(FUNewsManagementContext context) : base(context) { }

        public async Task<IEnumerable<NewsCategory>> GetAllWithParentAsync()
            => await _dbSet.Include(c => c.ParentCategory).ToListAsync();

        public async Task<IEnumerable<NewsCategory>> SearchAsync(string? searchTerm)
        {
            var query = _dbSet.Include(c => c.ParentCategory).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(c => c.CategoryName.Contains(searchTerm) ||
                                         (c.CategoryDescription != null && c.CategoryDescription.Contains(searchTerm)));

            return await query.ToListAsync();
        }

        public async Task<bool> HasArticlesAsync(short categoryId)
            => await _context.NewsArticles.AnyAsync(a => a.CategoryId == categoryId);
    }
}
