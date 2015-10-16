using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    internal abstract class Sertifikatvalidator
    {
        public X509Certificate2Collection SertifikatLager { get; set; }

        protected Sertifikatvalidator(X509Certificate2Collection sertifikatLager)
        {
            SertifikatLager = sertifikatLager;
        }

        public abstract bool ErGyldigResponssertifikat(X509Certificate2 sertifikat, out X509ChainStatus[] kjedestatus);

        public abstract bool ErGyldigResponssertifikat(X509Certificate2 sertifikat);
    }
}
