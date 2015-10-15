using System;
using System.Security.Cryptography.X509Certificates;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    internal class SertifikatValidatorQa : Sertifikatvalidator
    {
        public SertifikatValidatorQa(X509Certificate2Collection sertifikatLager) : base(sertifikatLager)
        {
        }

        public override X509ChainStatus[] ValiderResponssertifikat(X509Certificate2 sertifikat)
        {
            throw new System.NotImplementedException();
        }

        public override bool ErGyldigResponssertifikat(X509Certificate2 sertifikat)
        {
            throw new System.NotImplementedException();
        }

        public X509ChainPolicy ChainPolicyUtenRevokeringssjekkOgUkjentCertificateAuthority
        {
            get
            {
                var policy = new X509ChainPolicy()
                {
                    RevocationMode = X509RevocationMode.NoCheck,
                    UrlRetrievalTimeout = new TimeSpan(0, 1, 0),
                    VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority
                };
                //policy.ExtraStore.AddRange(X509Certificate2.TestSertifikater);

                return policy;

            }
        }
    }
}
