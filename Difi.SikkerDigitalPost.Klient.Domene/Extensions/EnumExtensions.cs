using System;
using Difi.Felles.Utility;

namespace Difi.SikkerDigitalPost.Klient.Domene.Extensions
{
    public static class EnumExtensions
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
                throw new ArgumentOutOfRangeException();
        }
    }

    }
}
