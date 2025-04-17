using InvoiceBillingSystem.Data;
using InvoiceBillingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBillingSystem.Repositories
{
    public class InvestmentRepository:IInvestmentRepository
    {
        private readonly ApplicationDbContext _context;

        public InvestmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTransactionAsync(InvestmentTransaction transaction)
        {
            await _context.InvestmentTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid UserId)
        {
            var user=await _context.Users.FirstOrDefaultAsync(e => e.Id == UserId);
            if(user == null)
             {
                return null;
             }
            return user;
        }
    }
}
