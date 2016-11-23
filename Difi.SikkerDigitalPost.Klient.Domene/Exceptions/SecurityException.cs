using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Exceptions
{
    [Serializable]
    public class SecurityException : SikkerDigitalPostException
    {
        public SecurityException()
        {
        }

        public SecurityException(string message)
            : base(message)
        {
        }

        public SecurityException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}