using System;
using System.Security.Cryptography.X509Certificates;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class Sertifikatbutikk
    {
        public X509Certificate2 Sertifikat(StoreName storeName, string thumbprint)
        {
            var store = new X509Store(storeName, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            store.Close();

            if (certificates.Count == 0)
                throw new Exception(string.Format("Fant ingen sertifikat i store {0} med oppgitt thumbprint {1}.", storeName, thumbprint));

            return certificates[0];
        }

        public X509Chain Chain(X509Certificate2 sertifikat)
        {
            var chain = new X509Chain();
            chain.Build(sertifikat);
            return chain;
        }
    }
}
