using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Exceptions
{
    [Serializable]
    public class XmlValidationException : KonfigurasjonsException
    {
        public XmlValidationException()
        {
        }

        public XmlValidationException(string message)
            : base(message)
        {
        }

        public XmlValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}