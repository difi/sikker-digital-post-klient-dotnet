using System;
using System.Collections.Generic;

namespace Difi.SikkerDigitalPost.Klient.Domene.Exceptions
{
    [Serializable]
    public class XmlValidationException : KonfigurasjonsException
    {
        public XmlValidationException(string message, List<string> validationMessages)
            : base(message)
        {
            ValidationMessages = validationMessages;
        }

        public List<string> ValidationMessages { get; private set; }
    }
}