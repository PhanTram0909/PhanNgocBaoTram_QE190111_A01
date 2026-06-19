using FUNewsManagementSystem.DataAccessLayer.Repositories;
using FUNewsManagementSystem.Model;

namespace FUNewsManagementSystem.BusinessLayer.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepo;

        public TagService(ITagRepository tagRepo)
        {
            _tagRepo = tagRepo;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
            => await _tagRepo.GetAllAsync();

        public async Task<Tag?> GetTagByIdAsync(int id)
            => await _tagRepo.GetByIdAsync(id);

        public async Task<IEnumerable<Tag>> SearchTagsAsync(string? searchTerm)
            => await _tagRepo.SearchAsync(searchTerm);

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            await _tagRepo.AddAsync(tag);
            return true;
        }

        public async Task<bool> UpdateTagAsync(Tag tag)
        {
            var existing = await _tagRepo.GetByIdAsync(tag.TagId);
            if (existing == null) return false;
            existing.TagName = tag.TagName;
            existing.Note = tag.Note;
            await _tagRepo.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            var tag = await _tagRepo.GetByIdAsync(id);
            if (tag == null) return false;
            await _tagRepo.DeleteAsync(tag);
            return true;
        }
    }
}
