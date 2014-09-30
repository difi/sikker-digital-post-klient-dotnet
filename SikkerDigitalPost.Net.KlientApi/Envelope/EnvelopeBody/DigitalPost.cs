using System.Xml;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody
{
    public class DigitalPost
    {
        private const string Ns9 = "http://begrep.difi.no/sdp/schema_v10";

        private readonly XmlDocument _dokument;

        public DigitalPost(XmlDocument dokument)
        {
            _dokument = dokument;
        }

        public XmlElement Xml()
        {
            var digitalPostElement = _dokument.CreateElement("ns9", "digitalPost", Ns9);



            return digitalPostElement;
        }

        private XmlElement SignatureElement()
        {
            return null;
        }

        private XmlElement AvsenderElement()
        {
            return null;
        }

        private XmlElement MottakerElement()
        {
            return null;
        }

        private XmlElement DigitalPostInfoElement()
        {
            return null;
        }

        private XmlElement DokumentfingerpakkeavtrykkElement()
        {
            return null;
        }
    }
}
