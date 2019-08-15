using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Exceptions
{
    [Serializable]
    public class SikkerDigitalPostException : Exception
    {
        public SikkerDigitalPostException()
        {
        }

        public SikkerDigitalPostException(string message)
            : base(message)
        {
        }

        public SikkerDigitalPostException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}