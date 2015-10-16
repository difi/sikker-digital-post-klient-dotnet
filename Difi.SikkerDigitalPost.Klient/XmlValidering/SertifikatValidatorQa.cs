using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Difi.SikkerDigitalPost.Klient.Extensions;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    internal class SertifikatValidatorQa : Sertifikatvalidator
    {
        public SertifikatValidatorQa(X509Certificate2Collection sertifikatLager) : base(sertifikatLager)
        {
        }

        public override X509ChainStatus[] ValiderResponssertifikat(X509Certificate2 sertifikat)
        {
            return Valider(sertifikat);
        }

        public override bool ErGyldigResponssertifikat(X509Certificate2 sertifikat)
        {
            return Valider(sertifikat).Length == 0;
        }

        private X509ChainStatus[] Valider(X509Certificate2 sertifikat)
        {
            var ignoreStoreMySertifikater = true;
            var chain = new X509Chain(ignoreStoreMySertifikater)
            {
                ChainPolicy = ChainPolicyUtenRevokeringssjekkOgUkjentCertificateAuthority()
            };
            chain.Build(sertifikat);

            return chain.ChainStatus;
        }

        private X509ChainPolicy ChainPolicyUtenRevokeringssjekkOgUkjentCertificateAuthority()
        {
            {
                var policy = new X509ChainPolicy()
                {
                    RevocationMode = X509RevocationMode.NoCheck,
                    UrlRetrievalTimeout = new TimeSpan(0, 1, 0),
                    VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority
                };
                policy.ExtraStore.AddRange(SertifikatLager);
                                
                return policy;

            }
        }
    }
}
