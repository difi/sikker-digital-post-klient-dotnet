using System;
using System.Management.Instrumentation;
using System.Security.Cryptography.X509Certificates;

namespace SikkerDigitalPost.Tester
{
    public class SertifikatUtility
    {
        public static X509Certificate2 AvsenderSertifkat(string hash)
        {
            X509Store storeMy = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            X509Certificate2 tekniskAvsenderSertifikat;
            try
            {
                storeMy.Open(OpenFlags.ReadOnly);
                tekniskAvsenderSertifikat = storeMy.Certificates.Find(
                    X509FindType.FindByThumbprint, hash, true)[0];
            }
            catch (Exception e)
            {
                throw new InstanceNotFoundException("Kunne ikke finne avsendersertifikat for testing. Har du lagt det til slik guiden tilsier? (https://github.com/difi/sikker-digital-post-net-klient#legg-inn-avsendersertifikat-i-certificate-store) ", e);
            }
            storeMy.Close();
            return tekniskAvsenderSertifikat;
        }

        public static X509Certificate2 MottakerSertifikat(string hash)
        {
            var storeTrusted = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
            X509Certificate2 mottakerSertifikat;
            try
            {
                storeTrusted.Open(OpenFlags.ReadOnly);
                mottakerSertifikat =
                    storeTrusted.Certificates.Find(X509FindType.FindByThumbprint, hash, true)[0];
            }
            catch (Exception e)
            {
                throw new InstanceNotFoundException("Kunne ikke finne mottakersertifikat for testing. Har du lagt det til slik guiden tilsier? (https://github.com/difi/sikker-digital-post-net-klient#legg-inn-mottakersertifikat-i-certificate-store) ", e);
            }
            storeTrusted.Close();
            return mottakerSertifikat;
        }
    }
}