using NotifyHub.Data;
using NotifyHub.Infrastructure.Repositories.Interfaces;
using NotifyHub.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NotifyHub.Infrastructure.Repositories.Implementations
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(c => c.CustomerEmails)  // Include related emails
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.CustomerEmails)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<CustomerEmail> GetCustomerEmailByIdAsync(int emailId)
        {
            return await _context.Set<CustomerEmail>()
                .FirstOrDefaultAsync(e => e.Id == emailId);
        }

        public void RemoveCustomerEmail(CustomerEmail email)
        {
            _context.Set<CustomerEmail>().Remove(email);
        }
    }
}