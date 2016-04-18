using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;

namespace Difi.SikkerDigitalPost.Klient.Internal
{
    internal class RequestHelper
    {
        public Klientkonfigurasjon ClientConfiguration { get; }

        public HttpClient HttpClient { get; set; }

        public RequestHelper(Klientkonfigurasjon clientConfiguration)
        {
            ClientConfiguration = clientConfiguration;
            HttpClient = new HttpClient(HttpClientHandlerChain());
        }

        private HttpMessageHandler HttpClientHandlerChain()
        {
            HttpClientHandler httpClientHandler;
            if (!string.IsNullOrEmpty(ClientConfiguration.ProxyHost))
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

      public async Task<Kvittering> Send(SoapContainer soapContainer)
        {
            //Todo: Ta inn forretningsmeldingEnvelope! Soap maa gjemmes bort
            
            var httpContent = CreateHttpContent(soapContainer);

            var responseMessage = await HttpClient.PostAsync(ClientConfiguration.Miljø.Url, httpContent);

            var data = await responseMessage.Content.ReadAsStringAsync();
          
            //Todo: Stygg hack for handtere void return. Splitt opp send etter forskjellige actions. Denne er for bekreft kvittering.
            if (data == string.Empty)
            {
                return null;
            }
            return KvitteringFactory.GetKvittering(data);

        }

        private HttpContent CreateHttpContent(SoapContainer container)
        {
            var meldingsinnhold = new MultipartFormDataContent(container.Boundary);
        
            //Todo: Dette er ullent og boer vaere kompilert kode.
            var contentType = string.Format(
                "Multipart/Related; boundary=\"{0}\"; " +
                "type=\"application/soap+xml\"; " +
                "start=\"<{1}>\"",
                container.Boundary,
                container.Envelope.ContentId);

            var mediaTypeHeaderValue = MediaTypeHeaderValue.Parse(contentType);
            meldingsinnhold.Headers.ContentType = mediaTypeHeaderValue;
            meldingsinnhold.Headers.Add("SOAPAction", "\"\"");

            LeggTilInnhold(container.Envelope, meldingsinnhold);
            foreach (var soapVedlegg in container.Vedlegg)
            {
                LeggTilInnhold(soapVedlegg, meldingsinnhold);
            }

            return meldingsinnhold;
        }

        private void LeggTilInnhold(ISoapVedlegg vedlegg, MultipartFormDataContent meldingsinnhold)
        {
            var meldingsdata = new ByteArrayContent(vedlegg.Bytes);

            var adjustedContentType = vedlegg.Innholdstype.Split(';')[0];

            meldingsdata.Headers.ContentType = new MediaTypeHeaderValue(adjustedContentType);
            meldingsdata.Headers.Add("Content-Transfer-Encoding", vedlegg.TransferEncoding);
            meldingsdata.Headers.Add("Content-ID", $"<{vedlegg.ContentId}>");

            meldingsinnhold.Add(meldingsdata);
        }
    }
}
