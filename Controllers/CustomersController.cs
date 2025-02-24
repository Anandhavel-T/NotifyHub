using NotifyHub.Infrastructure.Services.Interfaces;
using NotifyHub.Models.Domain;
using NotifyHub.Models.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;
using System.Linq;

namespace NotifyHub.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<ActionResult> Index()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return View(customers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please check the form and try again.";
                return RedirectToAction("Index");
            }

            try
            {
                var customer = new Customer
                {
                    Name = model.Name,
                    Phone = model.Phone,
                    ConnectionDetail = model.ConnectionDetail,
                    IsActive = model.IsActive,
                    CustomerEmails = model.CustomerEmails.Select(e => new CustomerEmail
                    {
                        Email = e.Email,
                        IsPrimary = e.IsPrimary,
                        IsActive = e.IsActive
                    }).ToList()
                };

                await _customerService.CreateCustomerAsync(customer);
                TempData["Success"] = "Customer created successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please check the form and try again.";
                return RedirectToAction("Index");
            }

            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(model.Id);
                if (customer == null)
                {
                    TempData["Error"] = "Customer not found.";
                    return RedirectToAction("Index");
                }

                customer.Name = model.Name;
                customer.Phone = model.Phone;
                customer.ConnectionDetail = model.ConnectionDetail;
                customer.IsActive = model.IsActive;
                customer.UpdatedAt = DateTime.UtcNow;

                // Update emails
                foreach (var emailModel in model.CustomerEmails)
                {
                    var existingEmail = customer.CustomerEmails.FirstOrDefault(e => e.Id == emailModel.Id);
                    if (existingEmail != null)
                    {
                        existingEmail.Email = emailModel.Email;
                        existingEmail.IsPrimary = emailModel.IsPrimary;
                        existingEmail.IsActive = emailModel.IsActive;
                    }
                    else
                    {
                        customer.CustomerEmails.Add(new CustomerEmail
                        {
                            Email = emailModel.Email,
                            IsPrimary = emailModel.IsPrimary,
                            IsActive = emailModel.IsActive
                        });
                    }
                }

                await _customerService.UpdateCustomerAsync(customer);
                TempData["Success"] = "Customer updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        public ActionResult AddEmailField(int? customerId)
        {
            var emailViewModel = new CustomerEmailViewModel();
            return PartialView("_CustomerEmailEditor", emailViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveEmail(int id)
        {
            await _customerService.RemoveCustomerEmailAsync(id);
            return RedirectToAction("Index");
        }
    }
}