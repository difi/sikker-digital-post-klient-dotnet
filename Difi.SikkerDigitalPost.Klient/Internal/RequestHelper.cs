using System;
using System.Collections.Generic;
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
using Difi.SikkerDigitalPost.Klient.Handlers;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;

namespace Difi.SikkerDigitalPost.Klient.Internal
{
    internal class RequestHelper
    {
        private static readonly ILog RequestResponseLog = LogManager.GetLogger("Difi.SikkerDigitalPost.Klient.RequestResponse");

        internal RequestHelper(Klientkonfigurasjon klientkonfigurasjon, params DelegatingHandler[] additionalHandlers)
        {
            ClientConfiguration = klientkonfigurasjon;
            Handlers.AddRange(additionalHandlers);
            HttpClient = HttpClientWithHandlerChain();
        }

        public RequestHelper(Klientkonfigurasjon klientkonfigurasjon)
        {
            ClientConfiguration = klientkonfigurasjon;
            HttpClient = HttpClientWithHandlerChain();
        }

        public Klientkonfigurasjon ClientConfiguration { get; }

        public HttpClient HttpClient { get; set; } //Todo: Hide this! Skal kun legge til handlere eller endre hele handlerlista.

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

        private List<DelegatingHandler> Handlers { get; } = new List<DelegatingHandler>
        {
            new UserAgentHandler()
        };
        
        private HttpClient HttpClientWithHandlerChain()
        {
            HttpClientHandler proxyOrNotHandler;
            if (ClientConfiguration.BrukProxy)
            {
                proxyOrNotHandler = new HttpClientHandler()
                {
                    Proxy = CreateProxy()
                };
            }
            else
            {
                proxyOrNotHandler = new HttpClientHandler();
            }

            var client = HttpClientFactory.Create(
                proxyOrNotHandler,
                Handlers.ToArray()
                );

            return client;
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

            var requestUri = RequestUri(envelope);
            var httpContent = CreateHttpContent(envelope, asiceDocumentBundle);

            var responseMessage = await HttpClient.PostAsync(requestUri, httpContent).ConfigureAwait(false);
            var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (ClientConfiguration.LoggForespørselOgRespons && RequestResponseLog.IsDebugEnabled)
            {
                RequestResponseLog.Debug($" Innkommende {responseContent}");
            }

            return responseContent;
        }

        private Uri RequestUri(AbstractEnvelope envelope)
        {
            var isOutgoingForsendelse = envelope.EnvelopeSettings.Forsendelse != null;
            return  isOutgoingForsendelse 
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