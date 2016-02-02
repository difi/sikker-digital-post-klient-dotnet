using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Exceptions
{
    [SerializableAttribute]
    public class XmlParseException : KonfigurasjonsException
    {
        private const string Ekstrainfo = " En mulig grunn til dette kan være at svar fra server har endret seg, eller at en feilaktig endring har blitt gjort.";
        private const string Ekstrainnerinfo = " Sjekk inner exception for mer detaljer.";

        public string Rådata { get; set; }

        public XmlParseException()
        {
            
        }

        public XmlParseException(string message) : base(message + Ekstrainfo)
        {
            
        }

        public XmlParseException(string message, Exception inner) 
            : base (message + Ekstrainfo + Ekstrainnerinfo, inner)
        {
            
        }

    }
}
