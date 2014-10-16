using System;

namespace SikkerDigitalPost.Domene.Exceptions
{
    class XmlParseException : KonfigurasjonsException
    {


        public XmlParseException()
        {
            
        }

        public XmlParseException(string message) : base(message)
        {
            
        }

        public XmlParseException(string message, Exception inner) 
            : base (message + "En mulig grunn til dette kan være at svar fra server har endret seg, eller at en feilaktig endring har blitt gjort.", inner)
        {
            
        }

    }
}
