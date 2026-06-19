using FUNewsManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.DataAccessLayer.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(FUNewsManagementContext context) : base(context) { }

        public async Task<IEnumerable<Tag>> SearchAsync(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await _dbSet.ToListAsync();

            return await _dbSet
                .Where(t => t.TagName.Contains(searchTerm) ||
                            (t.Note != null && t.Note.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Tag>> GetTagsByArticleAsync(string articleId)
            => await _context.NewsTags
                .Where(nt => nt.NewsArticleId == articleId)
                .Select(nt => nt.Tag!)
                .ToListAsync();
    }
}
