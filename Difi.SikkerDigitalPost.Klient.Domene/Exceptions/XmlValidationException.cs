using System;
using System.Collections.Generic;

namespace Difi.SikkerDigitalPost.Klient.Domene.Exceptions
{
    [Serializable]
    [Obsolete]
    public class XmlValidationException : SikkerDigitalPostException
    {
        public XmlValidationException(string message, List<string> validationMessages)
            : base(message + validationMessages)
        {
            ValidationMessages = validationMessages;
        }

        public XmlValidationException(string message, Exception inner)
             : base(message, inner)
        {
        }

        public XmlValidationException(string message)
             : base(message)
        {
            
        }

        public XmlValidationException()
        {
            
        }

        public List<string> ValidationMessages { get; private set; }
    }
}
