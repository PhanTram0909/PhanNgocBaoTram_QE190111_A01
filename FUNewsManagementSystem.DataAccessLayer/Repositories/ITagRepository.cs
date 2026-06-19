using FUNewsManagementSystem.Model;

namespace FUNewsManagementSystem.DataAccessLayer.Repositories
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        Task<IEnumerable<Tag>> SearchAsync(string? searchTerm);
        Task<IEnumerable<Tag>> GetTagsByArticleAsync(string articleId);
    }
}
