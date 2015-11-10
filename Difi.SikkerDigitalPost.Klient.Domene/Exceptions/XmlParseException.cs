using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Exceptions
{
    [SerializableAttribute]
    public class XmlParseException : KonfigurasjonsException
    {
        private const string ekstrainfo =
            " En mulig grunn til dette kan være at svar fra server har endret seg, eller at en feilaktig endring har blitt gjort.";

        private const string ekstrainnerinfo = " Sjekk inner exception for mer detaljer.";

        public string Rådata { get; set; }

        public XmlParseException()
        {
            
        }

        public XmlParseException(string message) : base(message + ekstrainfo)
        {
            
        }

        public XmlParseException(string message, Exception inner) 
            : base (message + ekstrainfo + ekstrainnerinfo, inner)
        {
            
        }

    }
}
