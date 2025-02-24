using Microsoft.AspNetCore.Cors;
using NotifyHub.Infrastructure.Services.Implementations;
using NotifyHub.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NotifyHub.Controllers
{
    public class NotificationAPIController : ApiController
    {
        private readonly INotificationService _notificationService;
        private readonly ICustomerService _customerService;

        public NotificationAPIController(INotificationService notificationService, ICustomerService customerService)
        {
            _notificationService = notificationService;
            _customerService = customerService;
        }

        [HttpGet]
        public HttpResponseMessage GetNotificationCount(int customerId)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK, 1);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            return response;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetNotifications()
        {
            try
            {
                // Get the referring domain from the request
                var referer = Request.Headers.Referrer;
                if (referer == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Referer header is required");
                }

                var domain = new Uri(referer.ToString()).Host;

                // Find customer by domain in ConnectionDetail
                var customer = _customerService.GetCustomerByDomain(domain);

                if (customer == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No customer found for this domain");
                }

                var notifications = await _notificationService.GetNotificationsForCustomer(customer.Id);

                var notificationDtos = notifications.Select(n => new
                {
                    Id = n.Id,
                    Subject = n.Subject,
                    ShortDescription = n.ShortDescription,
                    LongDescription = n.LongDescription,
                    CreatedAt = n.CreatedAt,
                    ExpiryAt = n.ExpiryAt,
                    ScheduledAt = n.ScheduledAt,
                    ImageUrl = n.ImageUrl,
                    Type = n.Type,
                    Priority = n.Priority,
                    IsRead = n.Recipients
                        .FirstOrDefault(r => r.CustomerId == customer.Id)?.IsRead ?? false,
                    IsDelivered = n.Recipients
                        .FirstOrDefault(r => r.CustomerId == customer.Id)?.IsDelivered ?? false
                }).ToList();

                var response = Request.CreateResponse(HttpStatusCode.OK, notificationDtos);

                //var response = Request.CreateResponse(HttpStatusCode.OK, notifications);

                // Add CORS headers
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Headers.Add("Access-Control-Allow-Methods", "GET, POST");
                response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

                return response;
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    "An error occurred while processing your request.");
            }
        }
    }

}
