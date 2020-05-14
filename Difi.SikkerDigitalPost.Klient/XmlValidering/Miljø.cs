using System;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    public class Miljø
    {
        public Miljø(Uri url)
        {
            var urlWithoutTrailingSlash = new Uri(url.ToString().TrimEnd('/'));
            Url = urlWithoutTrailingSlash;
        }

        public Uri Url { get; set; }

    }
}
