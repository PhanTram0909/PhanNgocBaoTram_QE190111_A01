using FUNewsManagementSystem.Model;

namespace FUNewsManagementSystem.BusinessLayer.Services
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag?> GetTagByIdAsync(int id);
        Task<IEnumerable<Tag>> SearchTagsAsync(string? searchTerm);
        Task<bool> CreateTagAsync(Tag tag);
        Task<bool> UpdateTagAsync(Tag tag);
        Task<bool> DeleteTagAsync(int id);
    }
}
