using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Exceptions
{
    [Serializable]
    public class KonfigurasjonsException : SikkerDigitalPostException
    {
        public KonfigurasjonsException()
        {
        }

        public KonfigurasjonsException(string message)
            : base(message)
        {
        }

        public KonfigurasjonsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}