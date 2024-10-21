using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BupaCodeAssesmentTests.Controllers
{
    public class TestHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _responseMessage;

        public TestHttpMessageHandler(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_responseMessage);
        }
    }
}
