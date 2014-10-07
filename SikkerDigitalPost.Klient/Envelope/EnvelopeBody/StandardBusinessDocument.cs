using System;
using System.Security.Cryptography.Xml;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klient.Xml;

namespace SikkerDigitalPost.Klient.Envelope.EnvelopeBody
{
    internal class StandardBusinessDocument : XmlPart
    {
        private readonly DateTime _creationDateAndtime;

        public StandardBusinessDocument(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
            _creationDateAndtime = DateTime.UtcNow;
        }

        public override XmlNode Xml()
        {
            var standardBusinessDocumentElement = Context.CreateElement("ns3", "StandardBusinessDocument", Navnerom.Ns3);
            standardBusinessDocumentElement.SetAttribute("xmlns:ns3", Navnerom.Ns3);
            standardBusinessDocumentElement.SetAttribute("xmlns:ns5", Navnerom.Ns5);
            standardBusinessDocumentElement.SetAttribute("xmlns:ns9", Navnerom.Ns9);

            standardBusinessDocumentElement.AppendChild(StandardBusinessDocumentHeaderElement());
            standardBusinessDocumentElement.AppendChild(DigitalPostElement());
           
            return standardBusinessDocumentElement;
        }

        private XmlNode StandardBusinessDocumentHeaderElement()
        {
            var standardBusinessDocumentHeader = new StandardBusinessDocumentHeader(Settings, Context, _creationDateAndtime);
            return standardBusinessDocumentHeader.Xml();
        }

        private XmlNode DigitalPostElement()
        {
            var digitalPost = new DigitalPostElement(Settings, Context);
            return digitalPost.Xml();
        }
    }
}
