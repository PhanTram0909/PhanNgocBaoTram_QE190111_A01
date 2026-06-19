using FUNewsManagementSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.DataAccessLayer.Repositories
{
    public class SystemAccountRepository : GenericRepository<SystemAccount>, ISystemAccountRepository
    {
        public SystemAccountRepository(FUNewsManagementContext context) : base(context) { }

        public async Task<SystemAccount?> GetByEmailAsync(string email)
            => await _dbSet.FirstOrDefaultAsync(a => a.AccountEmail.ToLower() == email.ToLower());

        public async Task<SystemAccount?> GetByEmailAndPasswordAsync(string email, string password)
            => await _dbSet.FirstOrDefaultAsync(a =>
                a.AccountEmail.ToLower() == email.ToLower() &&
                a.AccountPassword == password);

        public async Task<IEnumerable<SystemAccount>> SearchAsync(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await _dbSet.ToListAsync();

            return await _dbSet
                .Where(a => a.AccountName.Contains(searchTerm) ||
                            a.AccountEmail.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
