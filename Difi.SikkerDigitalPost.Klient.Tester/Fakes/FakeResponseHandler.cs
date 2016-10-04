using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Difi.SikkerDigitalPost.Klient.Tester.Fakes
{
    internal class FakeResponseHandler : DelegatingHandler
    {
        public Action<HttpRequestMessage> TestingAction { get; set; }

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public HttpContent HttpContent { get; set; } = new StringContent("Sucess");

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            TestingAction?.Invoke(requestMessage);

            var response = new HttpResponseMessage
            {
                Content = HttpContent,
                StatusCode = StatusCode
            };

            return await Task.FromResult(response).ConfigureAwait(false);
        }
    }
}