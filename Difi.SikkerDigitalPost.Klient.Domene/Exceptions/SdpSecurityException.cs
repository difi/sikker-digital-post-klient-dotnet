using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Exceptions
{
    [Serializable]
    public class SdpSecurityException : SikkerDigitalPostException
    {
        public SdpSecurityException()
        {
        }

        public SdpSecurityException(string message)
            : base(message)
        {
        }

        public SdpSecurityException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}