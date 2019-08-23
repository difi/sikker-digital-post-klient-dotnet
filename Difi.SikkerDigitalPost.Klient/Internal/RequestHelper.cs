using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding;
using Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsbekreftelse;
using Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsforespørsel;
using Difi.SikkerDigitalPost.Klient.Handlers;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using log4net;
using Microsoft.Extensions.Logging;

namespace Difi.SikkerDigitalPost.Klient.Internal
{
    internal class RequestHelper
    {
        private readonly ILogger<RequestHelper> _logger;
        private readonly ILoggerFactory _loggerFactory;
        
        public RequestHelper(Klientkonfigurasjon klientkonfigurasjon, ILoggerFactory loggerFactory):
            this(klientkonfigurasjon, loggerFactory, new DelegatingHandler[0])
        {
        }

        internal RequestHelper(Klientkonfigurasjon klientkonfigurasjon, ILoggerFactory loggerFactory, params DelegatingHandler[] additionalHandlers)
        {
            ClientConfiguration = klientkonfigurasjon;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<RequestHelper>();
            HttpClient = HttpClientWithHandlerChain(additionalHandlers);
        }

        public Klientkonfigurasjon ClientConfiguration { get; }

        public HttpClient HttpClient { get; set; }

        public async Task<Kvittering> SendMessage(ForretningsmeldingEnvelope envelope, DocumentBundle asiceDocumentBundle)
        {
            var result = await Send(envelope, asiceDocumentBundle).ConfigureAwait(false);

            return KvitteringFactory.GetKvittering(result);
        }

        public async Task<Kvittering> GetReceipt(KvitteringsforespørselEnvelope kvitteringsforespørselEnvelope)
        {
            var result = await Send(kvitteringsforespørselEnvelope).ConfigureAwait(false);

            return KvitteringFactory.GetKvittering(result);
        }

        public Task ConfirmReceipt(KvitteringsbekreftelseEnvelope kvitteringsbekreftelseEnvelope)
        {
            return Send(kvitteringsbekreftelseEnvelope);
        }

        private HttpClient HttpClientWithHandlerChain(IEnumerable<DelegatingHandler> additionalHandlers)
        {
            var proxyClientHandler = GetProxyOrDefaultHttpClientHandler();

            var allDelegatingHandlers = new List<DelegatingHandler> {new UserAgentHandler(), new LoggingHandler(ClientConfiguration, _loggerFactory)};
            allDelegatingHandlers.AddRange(additionalHandlers);

            var client = HttpClientFactory.Create(
                proxyClientHandler,
                allDelegatingHandlers.ToArray()
            );

            return client;
        }

        private HttpClientHandler GetProxyOrDefaultHttpClientHandler()
        {
            HttpClientHandler proxyOrNotHandler;
            if (ClientConfiguration.BrukProxy)
            {
                proxyOrNotHandler = new HttpClientHandler
                {
                    Proxy = CreateProxy()
                };
            }
            else
            {
                proxyOrNotHandler = new HttpClientHandler();
            }

            proxyOrNotHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            
            return proxyOrNotHandler;
        }

        private WebProxy CreateProxy()
        {
            var webProxy = new WebProxy(
                new UriBuilder(ClientConfiguration.ProxyScheme,
                    ClientConfiguration.ProxyHost, ClientConfiguration.ProxyPort).Uri, true);

            return webProxy;
        }

        private async Task<string> Send(AbstractEnvelope envelope, DocumentBundle asiceDocumentBundle = null)
        {
            if (ClientConfiguration.LoggForespørselOgRespons)
            {
                _logger.LogDebug($"Utgående {envelope.GetType().Name}, conversationId '{envelope.EnvelopeSettings.Forsendelse?.KonversasjonsId}', messageId '{envelope.EnvelopeSettings.GuidUtility.MessageId}': {envelope.Xml().OuterXml}");
            }

            var requestUri = RequestUri(envelope);
            var httpContent = CreateHttpContent(envelope, asiceDocumentBundle);

            var responseMessage = await HttpClient.PostAsync(requestUri, httpContent).ConfigureAwait(false);
            var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (ClientConfiguration.LoggForespørselOgRespons)
            {
                _logger.LogDebug($" Innkommende {responseContent}");
            }

            return responseContent;
        }

        private Uri RequestUri(AbstractEnvelope envelope)
        {
            var isOutgoingForsendelse = envelope.EnvelopeSettings.Forsendelse != null;
            return isOutgoingForsendelse
                ? ClientConfiguration.Miljø.UrlWithOrganisasjonsnummer(envelope.EnvelopeSettings.Databehandler.Organisasjonsnummer, envelope.EnvelopeSettings.Forsendelse.Avsender.Organisasjonsnummer)
                : ClientConfiguration.Miljø.Url;
        }

        private static HttpContent CreateHttpContent(AbstractEnvelope envelope, DocumentBundle asiceDocumentBundle)
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

        private static void AddEnvelopeToMultipart(ISoapVedlegg vedlegg, MultipartFormDataContent meldingsinnhold)
        {
            var byteArrayContent = new ByteArrayContent(vedlegg.Bytes);

            var adjustedContentType = vedlegg.Innholdstype.Split(';')[0];

            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(adjustedContentType);
            byteArrayContent.Headers.Add("Content-Transfer-Encoding", vedlegg.TransferEncoding);
            byteArrayContent.Headers.Add("Content-ID", $"<{vedlegg.ContentId}>");

            meldingsinnhold.Add(byteArrayContent);
        }

        private static void AddDocumentBundleToMultipart(DocumentBundle documentBundle, MultipartFormDataContent meldingsinnhold)
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