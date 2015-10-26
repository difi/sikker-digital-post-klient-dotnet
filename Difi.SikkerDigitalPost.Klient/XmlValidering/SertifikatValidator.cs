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

        public bool ErGyldigResponssertifikat(X509Certificate2 sertifikat)
        {
            X509ChainStatus[] kjedestatus;
            return ErGyldigResponssertifikat(sertifikat, out kjedestatus);
        }

        public bool ErGyldigResponssertifikat(X509Certificate2 sertifikat, out X509ChainStatus[] kjedestatus)
        {
            var ignoreStoreMySertifikater = true;
            var chain = new X509Chain(ignoreStoreMySertifikater)
            {
                ChainPolicy = ChainPolicy()
            };

            var erGyldigResponssertifikat = chain.Build(sertifikat);

            kjedestatus = chain.ChainStatus;
            return erGyldigResponssertifikat;
        }

        public abstract X509ChainPolicy ChainPolicy();
    }
}
