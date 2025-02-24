using NotifyHub.Infrastructure.Repositories.Interfaces;
using NotifyHub.Infrastructure.Services.Interfaces;
using NotifyHub.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NotifyHub.Infrastructure.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Task<Customer> CreateCustomerAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            // Ensure only one primary email
            var primaryEmails = customer.CustomerEmails?.Count(e => e.IsPrimary) ?? 0;
            if (primaryEmails > 1)
                throw new InvalidOperationException("Only one email can be marked as primary.");

            _customerRepository.Insert(customer);
            _customerRepository.SaveChangesAsync();
            return Task.FromResult(customer);
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                throw new NotFoundException($"Customer with ID {id} not found.");

            return customer;
        }

        public async Task RemoveCustomerEmailAsync(int emailId)
        {
            var email = await _customerRepository.GetCustomerEmailByIdAsync(emailId);
            if (email == null)
                throw new NotFoundException($"Customer email with ID {emailId} not found.");

            _customerRepository.RemoveCustomerEmail(email);
            await _customerRepository.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var existingCustomer = await _customerRepository.GetByIdAsync(customer.Id);
            if (existingCustomer == null)
                throw new NotFoundException($"Customer with ID {customer.Id} not found.");

            customer.UpdatedAt = DateTime.UtcNow;
            _customerRepository.Update(customer);
            await _customerRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public Task GetCustomerByDomain(string domain)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customer>> GetActiveCustomersAsync()
        {
            throw new NotImplementedException();
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}