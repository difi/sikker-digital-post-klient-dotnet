using System;
using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody;
using DigitalPost = SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody.DigitalPost;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.Body
{
    public class StandardBusinessDocument
    {
        private const string Ns3 = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader";
        private const string Ns5 = "http://www.w3.org/2000/09/xmldsig#";
        private const string Ns9 = "http://begrep.difi.no/sdp/schema_v10";
        
        private readonly XmlDocument _dokument;
        private readonly Forsendelse _forsendelse;
        private readonly DateTime _creationDateAndtime;
        
        public StandardBusinessDocument(XmlDocument dokument, Forsendelse forsendelse)
        {
            _forsendelse = forsendelse;
            _dokument = dokument;
            _creationDateAndtime = DateTime.UtcNow;
        }

        public XmlElement Xml()
        {
            var sbdElement = _dokument.CreateElement("ns3", "StandardBusinessDocument", Ns3);
            sbdElement.SetAttribute("xmlns:ns3", Ns3);
            sbdElement.SetAttribute("xmlns:ns5", Ns5);
            sbdElement.SetAttribute("xmlns:ns9", Ns9);

            var sbdHeader = new StandardBusinessDocumentHeader(_dokument, _forsendelse, _creationDateAndtime);
            sbdElement.AppendChild(sbdHeader.Xml());

            var digitalPost = new DigitalPost(_dokument, _forsendelse);
            sbdElement.AppendChild(digitalPost.Xml());

            return sbdElement;
        }
    }
}
