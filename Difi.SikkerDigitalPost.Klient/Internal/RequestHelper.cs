using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Handlers;
using Difi.SikkerDigitalPost.Klient.SBDH;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;
using StandardBusinessDocument = Difi.SikkerDigitalPost.Klient.SBDH.StandardBusinessDocument;

namespace Difi.SikkerDigitalPost.Klient.Internal
{
    internal class RequestHelper
    {
        private readonly ILogger<RequestHelper> _logger;
        private readonly ILoggerFactory _loggerFactory;

        public RequestHelper(Klientkonfigurasjon klientkonfigurasjon, ILoggerFactory loggerFactory) :
            this(klientkonfigurasjon, loggerFactory, new DelegatingHandler[0])
        {
        }

        internal RequestHelper(Klientkonfigurasjon klientkonfigurasjon, ILoggerFactory loggerFactory,
            params DelegatingHandler[] additionalHandlers)
        {
            ClientConfiguration = klientkonfigurasjon;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<RequestHelper>();
            HttpClient = HttpClientWithHandlerChain(additionalHandlers);
        }

        public Klientkonfigurasjon ClientConfiguration { get; }

        public HttpClient HttpClient { get; set; }

        public async Task<Transportkvittering> SendMessage(StandardBusinessDocument standardBusinessDocument,
            Dokumentpakke dokumentpakke, MetadataDocument metadataDocument)
        {
            var openRequestUri = new Uri(ClientConfiguration.Miljø.Url, $"messages/out/");
            var putRequestUri = new Uri(openRequestUri,
                $"{standardBusinessDocument.standardBusinessDocumentHeader.businessScope.scope[0].instanceIdentifier}");

            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
            };
            
            string json = JsonSerializer.Serialize(standardBusinessDocument, standardBusinessDocument.GetType(), jsonSerializerOptions);

            JObject sbdobj = JObject.Parse(json);
            sbdobj.Add(standardBusinessDocument.any is DigitalForretningsMelding ? "digital" : "print", sbdobj["any"]);
            sbdobj.Remove("any");

            string newjson = sbdobj.ToString();

            StringContent content = new StringContent(newjson, Encoding.UTF8, "application/json");

            var responseMessage = await HttpClient.PostAsync(openRequestUri, content).ConfigureAwait(false);
            var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!responseMessage.IsSuccessStatusCode)
            {
                return new TransportFeiletKvittering
                {
                    Skyldig = ((int)responseMessage.StatusCode).ToString().StartsWith("4") ? Feiltype.Klient : Feiltype.Server,
                    Beskrivelse = responseMessage.ReasonPhrase,
                    Feilkode = responseMessage.StatusCode.ToString()
                };
            }
            
            Transportkvittering hovedAddKvittering = await addDocument(dokumentpakke.Hoveddokument, putRequestUri);

            if (hovedAddKvittering is TransportFeiletKvittering)
            {
                return hovedAddKvittering;
            }
            
            if (metadataDocument != null)
            { 
                Transportkvittering addKvittering = await addDocument(metadataDocument, putRequestUri);

                if (addKvittering is TransportFeiletKvittering)
                {
                    return addKvittering;
                }
            }
            
            foreach (Dokument vedlegg in dokumentpakke.Vedlegg)
            {
                Transportkvittering addKvittering = await addDocument(vedlegg, putRequestUri);
                
                if (addKvittering is TransportFeiletKvittering)
                {
                    return addKvittering;
                }
            }

            responseMessage = await HttpClient.PostAsync(putRequestUri, null).ConfigureAwait(false);
            responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!responseMessage.IsSuccessStatusCode)
            {
                return new TransportFeiletKvittering
                {
                    Skyldig = ((int)responseMessage.StatusCode).ToString().StartsWith("4") ? Feiltype.Klient : Feiltype.Server,
                    Beskrivelse = responseMessage.ReasonPhrase,
                    Feilkode = responseMessage.StatusCode.ToString()
                };
            }
            
            if (ClientConfiguration.LoggForespørselOgRespons)
            {
                _logger.LogDebug($" Innkommende {responseContent}");
            }

            return new TransportOkKvittering();
        }
        
        private async Task<Transportkvittering> addDocument(IWithDocumentProperties document, Uri putRequestUri)
        {
            var docContent = new ByteArrayContent(document.Bytes);
            docContent.Headers.Add("content-type", document.MimeType);

            var vedleggContentDisposition = $"attachment; filename=\"{document.Filnavn}\"";
            vedleggContentDisposition += document.Tittel == null ? "" : $"; name=\"{document.Tittel}\"";
            docContent.Headers.Add("content-disposition", vedleggContentDisposition);

            var responseMessage = await HttpClient.PutAsync(putRequestUri, docContent).ConfigureAwait(false);
            var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!responseMessage.IsSuccessStatusCode)
            {
                return new TransportFeiletKvittering
                {
                    Skyldig = ((int)responseMessage.StatusCode).ToString().StartsWith("4") ? Feiltype.Klient : Feiltype.Server,
                    Beskrivelse = responseMessage.ReasonPhrase,
                    Feilkode = responseMessage.StatusCode.ToString()
                };
            }
            
            _logger.LogInformation(responseContent);
            
            return new TransportOkKvittering();
        }

        public async Task<IntegrasjonspunktKvittering> GetReceipt()
        {
            var uri = new Uri(ClientConfiguration.Miljø.Url, $"statuses/peek");
            
            var responseMessage = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }
            
            return JsonSerializer.Deserialize<IntegrasjonspunktKvittering>(responseContent);
        }
        
        public async Task<string> ConfirmReceipt(long id)
        {
            var uri = new Uri(ClientConfiguration.Miljø.Url, $"statuses/{id}");
            
            var responseMessage = await HttpClient.DeleteAsync(uri).ConfigureAwait(false);
            return await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private HttpClient HttpClientWithHandlerChain(IEnumerable<DelegatingHandler> additionalHandlers)
        {
            var proxyClientHandler = GetProxyOrDefaultHttpClientHandler();

            var allDelegatingHandlers = new List<DelegatingHandler>
                {new UserAgentHandler(), new LoggingHandler(ClientConfiguration, _loggerFactory)};
            allDelegatingHandlers.AddRange(additionalHandlers);

            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

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

            proxyOrNotHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                return true;
            };
            proxyOrNotHandler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;

            return proxyOrNotHandler;
        }

        private WebProxy CreateProxy()
        {
            var webProxy = new WebProxy(
                new UriBuilder(ClientConfiguration.ProxyScheme,
                    ClientConfiguration.ProxyHost, ClientConfiguration.ProxyPort).Uri, true);

            return webProxy;
        }
    }
}
