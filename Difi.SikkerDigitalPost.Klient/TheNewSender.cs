using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;

namespace Difi.SikkerDigitalPost.Klient
{
    internal class TheNewSender
    {

        internal async Task<HttpResponseMessage> Send(SoapContainer container, string url, int timoutIMillisekunder)
        {
            HttpContent innhold = GenererInnhold(container);HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(timoutIMillisekunder);
            client.DefaultRequestHeaders.Add("Accept", "*/*");

            return await client.PostAsync(url, innhold);
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
    }
}
