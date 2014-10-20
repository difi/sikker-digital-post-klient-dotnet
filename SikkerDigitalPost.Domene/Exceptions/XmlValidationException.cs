using System;

namespace SikkerDigitalPost.Domene.Exceptions
{
    class XmlValidationException : KonfigurasjonsException
    {
        private const string Ekstrainfo = " Validering av Xml feilet. Se inner exception for mer info.";

        public XmlValidationException()
        {
            
        }

        public XmlValidationException(string message) : base(message + Ekstrainfo)
        {
            
        }

        public XmlValidationException(string message, Exception inner) 
            : base (message + Ekstrainfo, inner)
        {
            
        }
    }
}
