using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Exceptions
{
    [SerializableAttribute]
    public class XmlValidationException : KonfigurasjonsException
    {
        private const string StartAvMelding = " Validering av Xml feilet: ";

        public XmlValidationException()
        {
            
        }

        public XmlValidationException(string message) : base(StartAvMelding + message)
        {
            
        }

        public XmlValidationException(string message, Exception inner) 
            : base (message + StartAvMelding, inner)
        {
            
        }
    }
}
