using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;

namespace Difi.SikkerDigitalPost.Klient
{
    internal class Meldingshandling
    {
        private readonly Klientkonfigurasjon _klientkonfigurasjon;

        internal Meldingshandling(Klientkonfigurasjon klientkonfigurasjon)
        {
            _klientkonfigurasjon = klientkonfigurasjon;
        }

        internal async Task<HttpResponseMessage> Send(SoapContainer container)
        {
            var innhold = GenererInnhold(container);
            
            GetThreadSafeClient.DefaultRequestHeaders.Add("Accept", "*/*");
            return await GetThreadSafeClient.PostAsync(_klientkonfigurasjon.Miljø.Url, innhold);
        }

        private HttpContent GenererInnhold(SoapContainer container)
        {
            MultipartFormDataContent meldingsinnhold = new MultipartFormDataContent(container.Boundary);
            
            var contentType =string.Format(
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
            meldingsdata.Headers.Add("Content-ID", string.Format("<{0}>", vedlegg.ContentId));
            
            meldingsinnhold.Add(meldingsdata);
        }

        private readonly object _threadLock = new Object();

        private HttpClient _httpClient;

        private HttpClient GetThreadSafeClient
        {
            get
            {
                lock (_threadLock)
                {
                    if (_httpClient != null) return _httpClient;

                    var timeout = TimeSpan.FromMilliseconds(_klientkonfigurasjon.TimeoutIMillisekunder);

                    if (_klientkonfigurasjon.BrukProxy)
                    {

                        var proxyHandler = new HttpClientHandler()
                        {
                            Proxy = new WebProxy(_klientkonfigurasjon.ProxyHost, _klientkonfigurasjon.ProxyPort)
                        };

                        _httpClient = new HttpClient(proxyHandler)
                        {
                            Timeout = timeout
                        };
                    }
                    else
                    {
                        _httpClient = new HttpClient
                        {
                            Timeout = timeout
                        };
                    }

                    return _httpClient;
                }
            }
        }
    }
}
