using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    internal abstract class Sertifikatvalidator
    {
        public X509Certificate2 Sertifikat { get; set; }
        public IEnumerable<X509Certificate2> SertifikatLager { get; set; }

        protected Sertifikatvalidator(IEnumerable<X509Certificate2> sertifikatLager)
        {
            SertifikatLager = sertifikatLager;
            throw new NotImplementedException();
        }

        public abstract X509ChainStatus[] ValiderRespons(X509Certificate2 sertifikat);
    }
}
