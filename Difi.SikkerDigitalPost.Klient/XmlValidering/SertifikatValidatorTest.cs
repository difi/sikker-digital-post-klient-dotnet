using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Difi.SikkerDigitalPost.Klient.Extensions;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    internal class SertifikatValidatorTest : Sertifikatvalidator
    {
        public SertifikatValidatorTest(X509Certificate2Collection sertifikatLager) : base(sertifikatLager)
        {
        }
        public override bool ErGyldigResponssertifikat(X509Certificate2 sertifikat)
        {
            X509ChainStatus[] kjedestatus;
            return ErGyldigResponssertifikat(sertifikat, out kjedestatus);
        }

        public override bool ErGyldigResponssertifikat(X509Certificate2 sertifikat, out X509ChainStatus[] kjedestatus)
        {
            var ignoreStoreMySertifikater = true;
            var chain = new X509Chain(ignoreStoreMySertifikater)
            {
                ChainPolicy = ChainPolicyUtenRevokeringssjekkOgUkjentCertificateAuthority()
            };

            bool gyldigSertifikat = chain.Build(sertifikat);

            kjedestatus = chain.ChainStatus;
            return gyldigSertifikat;
        }

        private X509ChainPolicy ChainPolicyUtenRevokeringssjekkOgUkjentCertificateAuthority()
        {
            {
                var policy = new X509ChainPolicy()
                {
                    RevocationMode = X509RevocationMode.NoCheck,
                    VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority
                };
                policy.ExtraStore.AddRange(SertifikatLager);
                                
                return policy;

            }
        }
    }
}
