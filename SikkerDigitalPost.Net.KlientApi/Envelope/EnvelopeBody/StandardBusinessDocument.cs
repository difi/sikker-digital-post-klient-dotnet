using System;
using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Entiteter.Aktører;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody
{
    public class StandardBusinessDocument : XmlPart
    {
        private readonly DateTime _creationDateAndtime;
        
        public StandardBusinessDocument(XmlDocument dokument, Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler) : base(dokument,forsendelse, asicEArkiv, databehandler)
        {
            _creationDateAndtime = DateTime.UtcNow;
        }

        public override XmlElement Xml()
        {
            var standardBusinessDocument = XmlEnvelope.CreateElement("ns3", "StandardBusinessDocument", Navnerom.Ns3);
            standardBusinessDocument.SetAttribute("xmlns:ns3", Navnerom.Ns3);
            standardBusinessDocument.SetAttribute("xmlns:ns5", Navnerom.Ns5);
            standardBusinessDocument.SetAttribute("xmlns:ns9", Navnerom.Ns9);

            standardBusinessDocument.AppendChild(StandardBusinessDocumentHeaderElement());
            standardBusinessDocument.AppendChild(DigitalPostElement());

            return standardBusinessDocument;
        }

        private XmlElement StandardBusinessDocumentHeaderElement()
        {
            var standardBusinessDocumentHeader = new StandardBusinessDocumentHeader(
                XmlEnvelope, Forsendelse, AsicEArkiv, Databehandler, _creationDateAndtime);
            return standardBusinessDocumentHeader.Xml();
        }

        private XmlElement DigitalPostElement()
        {
            var digitalPost = new DigitalPostElement(XmlEnvelope, Forsendelse, AsicEArkiv, Databehandler);
            return digitalPost.Xml();
        }
    }
}
