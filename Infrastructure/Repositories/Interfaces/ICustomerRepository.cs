using NotifyHub.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifyHub.Infrastructure.Repositories.Interfaces
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<Customer> GetByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<bool> SaveChangesAsync();
        Task<CustomerEmail> GetCustomerEmailByIdAsync(int emailId);
        void RemoveCustomerEmail(CustomerEmail email);
    }
}
