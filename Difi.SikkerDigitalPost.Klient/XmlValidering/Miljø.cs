using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    public class Miljø
    {
        public IEnumerable<X509Certificate2> Sertifikater { get; set; }

        public Url Url { get; set; }

        internal Sertifikatvalidator Sertifikatvalidator { get; set; }

        private Miljø(IEnumerable<X509Certificate2> sertifikater, Url url)
        {
            Sertifikater = sertifikater;
            Url = url;
        }

        public Miljø Produksjon
        {
            get
            {
                return new Miljø(new List<X509Certificate2>(), new Url(""));
            }
        }

        public Miljø Test
        {
            get
            {
                return new Miljø(new List<X509Certificate2>(), new Url(""));
            }
        }
    }
}
