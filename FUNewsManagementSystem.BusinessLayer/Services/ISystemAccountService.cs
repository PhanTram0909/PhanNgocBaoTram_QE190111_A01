using FUNewsManagementSystem.Model;

namespace FUNewsManagementSystem.BusinessLayer.Services
{
    public interface ISystemAccountService
    {
        Task<IEnumerable<SystemAccount>> GetAllAccountsAsync();
        Task<SystemAccount?> GetAccountByIdAsync(short id);
        Task<SystemAccount?> GetAccountByEmailAsync(string email);
        Task<SystemAccount?> LoginAsync(string email, string password);
        Task<IEnumerable<SystemAccount>> SearchAccountsAsync(string? searchTerm);
        Task<bool> CreateAccountAsync(SystemAccount account);
        Task<bool> UpdateAccountAsync(SystemAccount account);
        Task<bool> DeleteAccountAsync(short id);
        Task<bool> EmailExistsAsync(string email, short? excludeId = null);
    }
}
