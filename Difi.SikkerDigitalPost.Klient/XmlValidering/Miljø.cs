using System;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    public class Miljø
    {
        public Miljø(Uri url)
        {
            Url = url;
        }

        public Uri Url { get; set; }

        public static Miljø IntegrasjonsPunktLocalHostMiljø => new Miljø(new Uri("http://127.0.0.1:9093/api/"));
    }
}
