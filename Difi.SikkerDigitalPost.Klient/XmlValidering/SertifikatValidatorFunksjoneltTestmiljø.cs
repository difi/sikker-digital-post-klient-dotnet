using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Difi.SikkerDigitalPost.Klient.Extensions;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    internal class SertifikatValidatorFunksjoneltTestmiljø : Sertifikatvalidator
    {
        public SertifikatValidatorFunksjoneltTestmiljø(X509Certificate2Collection sertifikatLager) : base(sertifikatLager)
        {
        }

        public override X509ChainPolicy ChainPolicy()
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
