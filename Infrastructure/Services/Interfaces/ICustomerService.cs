using NotifyHub.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifyHub.Infrastructure.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByIdAsync(int id);
        Task RemoveCustomerEmailAsync(int emailId);
        Task UpdateCustomerAsync(Customer customer);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task GetCustomerByDomain(string domain);
        Task<IEnumerable<Customer>> GetActiveCustomersAsync();
    }
}
