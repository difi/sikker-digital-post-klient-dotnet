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
            string data;

            var httpContent = CreateHttpContent(soapContainer);

            //GetThreadSafeClient.DefaultRequestHeaders.Add("Accept", "*/*");

            var responseMessage = await HttpClient.PostAsync(ClientConfiguration.Miljø.Url, httpContent);

            try
            {
                data = await responseMessage.Content.ReadAsStringAsync();
            }
            catch (WebException we)
            {
                using (var response = we.Response as HttpWebResponse)
                {
                    if (response == null)
                    {
                        throw new SendException("Får ikke kontakt med meldingsformidleren. Sjekk tilkoblingsdetaljer og prøv på nytt.");
                    }


                    using (var errorStream = response.GetResponseStream())
                    {
                        var soap = XDocument.Load(errorStream);
                        data = soap.ToString();

                    }
                }
            }

            if (data == string.Empty)
            {
                return null;
            }
            return KvitteringFactory.GetKvittering(data);

        }

        private HttpContent CreateHttpContent(SoapContainer container)
        {
            var meldingsinnhold = new MultipartFormDataContent(container.Boundary);

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
