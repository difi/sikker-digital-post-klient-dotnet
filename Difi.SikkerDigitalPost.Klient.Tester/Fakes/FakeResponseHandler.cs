using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Difi.SikkerDigitalPost.Klient.Tester.Fakes
{
    internal class FakeResponseHandler : DelegatingHandler
    {
        private readonly Action<HttpRequestMessage> _testingAction;

        public FakeResponseHandler(Action<HttpRequestMessage> testingAction)
        {
            _testingAction = testingAction;
        }

        public FakeResponseHandler()
        {
        }

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public HttpContent HttpContent { get; set; } = new StringContent("Sucess");

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            _testingAction?.Invoke(requestMessage);

            var response = new HttpResponseMessage
            {
                Content = HttpContent,
                StatusCode = StatusCode
            };

            return await Task.FromResult(response).ConfigureAwait(false);
        }
    }
}