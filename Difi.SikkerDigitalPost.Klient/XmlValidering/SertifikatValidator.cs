using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    internal abstract class Sertifikatvalidator
    {
        public X509Certificate2 Sertifikat { get; set; }
        public X509Certificate2Collection SertifikatLager { get; set; }

        protected Sertifikatvalidator(X509Certificate2Collection sertifikatLager)
        {
            SertifikatLager = sertifikatLager;
            throw new NotImplementedException();
        }

        public abstract X509ChainStatus[] ValiderResponssertifikat(X509Certificate2 sertifikat);

        public abstract bool ErGyldigResponssertifikat(X509Certificate2 sertifikat);
    }
}
