using System;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;

namespace SikkerDigitalPost.Klient.Envelope.EnvelopeBody
{
    internal class StandardBusinessDocument : XmlPart
    {
        private readonly DateTime _creationDateAndtime;
        
        public StandardBusinessDocument(Envelope rot) : base(rot)
        {
           _creationDateAndtime = DateTime.UtcNow;
        }

        public override XmlElement Xml()
        {
            var standardBusinessDocument = Rot.EnvelopeXml.CreateElement("ns3", "StandardBusinessDocument", Navnerom.Ns3);
            standardBusinessDocument.SetAttribute("xmlns:ns3", Navnerom.Ns3);
            standardBusinessDocument.SetAttribute("xmlns:ns5", Navnerom.Ns5);
            standardBusinessDocument.SetAttribute("xmlns:ns9", Navnerom.Ns9);

            standardBusinessDocument.AppendChild(StandardBusinessDocumentHeaderElement());
            standardBusinessDocument.AppendChild(DigitalPostElement());

            return standardBusinessDocument;
        }

        private XmlElement StandardBusinessDocumentHeaderElement()
        {
            var standardBusinessDocumentHeader = new StandardBusinessDocumentHeader(Rot, _creationDateAndtime);
            return standardBusinessDocumentHeader.Xml();
        }

        private XmlElement DigitalPostElement()
        {
            var digitalPost = new DigitalPostElement(Rot);
            return digitalPost.Xml();
        }
    }
}
