using System;
using Digipost.Api.Client.Shared.Certificate;

namespace Difi.SikkerDigitalPost.Klient.Domene.Extensions
{
    internal static class EnumExtensions
    {
        public static string ToNorwegianString(this CertificateValidationType certificateValidationType)
        {
            switch (certificateValidationType)
            {
                case CertificateValidationType.Valid:
                    return "Gyldig";
                case CertificateValidationType.InvalidCertificate:
                    return "UgyldigSertifikat";
                case CertificateValidationType.InvalidChain:
                    return "UgyldigKjede";
                default:
                    throw new ArgumentOutOfRangeException(nameof(certificateValidationType), certificateValidationType, null);
            }
        }

    }
}
