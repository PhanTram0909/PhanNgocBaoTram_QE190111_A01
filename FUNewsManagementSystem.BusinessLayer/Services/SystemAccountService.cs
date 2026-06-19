using FUNewsManagementSystem.DataAccessLayer.Repositories;
using FUNewsManagementSystem.Model;

namespace FUNewsManagementSystem.BusinessLayer.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _accountRepo;

        public SystemAccountService(ISystemAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task<IEnumerable<SystemAccount>> GetAllAccountsAsync()
            => await _accountRepo.GetAllAsync();

        public async Task<SystemAccount?> GetAccountByIdAsync(short id)
            => await _accountRepo.GetByIdAsync(id);

        public async Task<SystemAccount?> GetAccountByEmailAsync(string email)
            => await _accountRepo.GetByEmailAsync(email);

        public async Task<SystemAccount?> LoginAsync(string email, string password)
            => await _accountRepo.GetByEmailAndPasswordAsync(email, password);

        public async Task<IEnumerable<SystemAccount>> SearchAccountsAsync(string? searchTerm)
            => await _accountRepo.SearchAsync(searchTerm);

        public async Task<bool> CreateAccountAsync(SystemAccount account)
        {
            if (await EmailExistsAsync(account.AccountEmail))
                return false;
            await _accountRepo.AddAsync(account);
            return true;
        }

        public async Task<bool> UpdateAccountAsync(SystemAccount account)
        {
            var existing = await _accountRepo.GetByIdAsync(account.AccountId);
            if (existing == null) return false;

            existing.AccountName = account.AccountName;
            existing.AccountEmail = account.AccountEmail;
            existing.AccountRole = account.AccountRole;
            if (!string.IsNullOrWhiteSpace(account.AccountPassword))
                existing.AccountPassword = account.AccountPassword;

            await _accountRepo.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAccountAsync(short id)
        {
            var account = await _accountRepo.GetByIdAsync(id);
            if (account == null) return false;
            await _accountRepo.DeleteAsync(account);
            return true;
        }

        public async Task<bool> EmailExistsAsync(string email, short? excludeId = null)
        {
            var account = await _accountRepo.GetByEmailAsync(email);
            if (account == null) return false;
            if (excludeId.HasValue && account.AccountId == excludeId.Value) return false;
            return true;
        }
    }
}
