using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Common.Logging;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding;
using Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsbekreftelse;
using Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsforespørsel;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;

namespace Difi.SikkerDigitalPost.Klient.Internal
{
    internal class RequestHelper
    {
        private static readonly ILog RequestResponseLog = LogManager.GetLogger("Difi.SikkerDigitalPost.Klient.RequestResponse");

        public RequestHelper(Klientkonfigurasjon clientConfiguration)
        {
            ClientConfiguration = clientConfiguration;
            HttpClient = new HttpClient(HttpClientHandlerChain());
        }

        public Klientkonfigurasjon ClientConfiguration { get; }

        public HttpClient HttpClient { get; set; }

        public async Task<Kvittering> SendMessage(ForretningsmeldingEnvelope envelope, DocumentBundle asiceDocumentBundle)
        {
            var result = await Send(envelope, asiceDocumentBundle);

            return KvitteringFactory.GetKvittering(result);
        }

        public async Task<Kvittering> GetReceipt(KvitteringsforespørselEnvelope kvitteringsforespørselEnvelope)
        {
            var result = await Send(kvitteringsforespørselEnvelope);

            return KvitteringFactory.GetKvittering(result);
        }

        public Task ConfirmReceipt(KvitteringsbekreftelseEnvelope kvitteringsbekreftelseEnvelope)
        {
            return Send(kvitteringsbekreftelseEnvelope);
        }

        private HttpMessageHandler HttpClientHandlerChain()
        {
            HttpClientHandler httpClientHandler;
            if (ClientConfiguration.BrukProxy)
            {
                httpClientHandler = new HttpClientHandler
                {
                    Proxy = CreateProxy()
                };
            }
            else
            {
                httpClientHandler = new HttpClientHandler();
            }

            return httpClientHandler;
        }

        private WebProxy CreateProxy()
        {
            return new WebProxy(
                new UriBuilder(ClientConfiguration.ProxyScheme,
                    ClientConfiguration.ProxyHost, ClientConfiguration.ProxyPort).Uri);
        }

        private async Task<string> Send(AbstractEnvelope envelope, DocumentBundle asiceDocumentBundle = null)
        {
            if (ClientConfiguration.LoggForespørselOgRespons && RequestResponseLog.IsDebugEnabled)
            {
                RequestResponseLog.Debug($"Utgående {envelope.GetType().Name}, conversationId '{envelope.EnvelopeSettings.Forsendelse?.KonversasjonsId}', messageId '{envelope.EnvelopeSettings.GuidUtility.MessageId}': {envelope.Xml().OuterXml}");
            }

            var httpContent = CreateHttpContent(envelope, asiceDocumentBundle);
            var responseMessage = await HttpClient.PostAsync(ClientConfiguration.Miljø.Url, httpContent);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            if (ClientConfiguration.LoggForespørselOgRespons && RequestResponseLog.IsDebugEnabled)
            {
                RequestResponseLog.Debug($" Innkommende {responseContent}");
            }

            return responseContent;
        }

        private HttpContent CreateHttpContent(AbstractEnvelope envelope, DocumentBundle asiceDocumentBundle)
        {
            var boundary = Guid.NewGuid().ToString();
            var multipartFormDataContent = new MultipartFormDataContent(boundary);

            var contentType = $"Multipart/Related; boundary=\"{boundary}\"; " + "type=\"application/soap+xml\"; " + $"start=\"<{envelope.ContentId}>\"";

            var mediaTypeHeaderValue = MediaTypeHeaderValue.Parse(contentType);
            multipartFormDataContent.Headers.ContentType = mediaTypeHeaderValue;

            AddEnvelopeToMultipart(envelope, multipartFormDataContent);
            AddDocumentBundleToMultipart(asiceDocumentBundle, multipartFormDataContent);

            return multipartFormDataContent;
        }

        private void AddEnvelopeToMultipart(ISoapVedlegg vedlegg, MultipartFormDataContent meldingsinnhold)
        {
            var byteArrayContent = new ByteArrayContent(vedlegg.Bytes);

            var adjustedContentType = vedlegg.Innholdstype.Split(';')[0];

            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(adjustedContentType);
            byteArrayContent.Headers.Add("Content-Transfer-Encoding", vedlegg.TransferEncoding);
            byteArrayContent.Headers.Add("Content-ID", $"<{vedlegg.ContentId}>");

            meldingsinnhold.Add(byteArrayContent);
        }

        private void AddDocumentBundleToMultipart(DocumentBundle documentBundle, MultipartFormDataContent meldingsinnhold)
        {
            if (documentBundle != null)
            {
                var meldingsdata = new ByteArrayContent(documentBundle.BundleBytes);

                meldingsdata.Headers.ContentType = new MediaTypeHeaderValue(documentBundle.ContentType);
                meldingsdata.Headers.Add("Content-Transfer-Encoding", documentBundle.TransferEncoding);
                meldingsdata.Headers.Add("Content-ID", $"<{documentBundle.ContentId}>");

                meldingsinnhold.Add(meldingsdata);
            }
        }
    }
}