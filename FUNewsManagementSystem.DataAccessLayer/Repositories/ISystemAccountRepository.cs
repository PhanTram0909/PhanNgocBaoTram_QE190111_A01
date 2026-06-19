using FUNewsManagementSystem.Model;

namespace FUNewsManagementSystem.DataAccessLayer.Repositories
{
    public interface ISystemAccountRepository : IGenericRepository<SystemAccount>
    {
        Task<SystemAccount?> GetByEmailAsync(string email);
        Task<SystemAccount?> GetByEmailAndPasswordAsync(string email, string password);
        Task<IEnumerable<SystemAccount>> SearchAsync(string? searchTerm);
    }
}
