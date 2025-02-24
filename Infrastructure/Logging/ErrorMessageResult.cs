using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Web.Http;

namespace NotifyHub.Infrastructure.Logging
{
    public class ErrorMessageResult : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly HttpResponseMessage _response;

        public ErrorMessageResult(HttpRequestMessage request, HttpResponseMessage response)
        {
            _request = request;
            _response = response;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }
}