using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klientbibliotek.Utilities;

namespace SikkerDigitalPost.Klientbibliotek.Envelope.EnvelopeBody
{

    internal class Body : XmlPart
    {
        public Body(XmlDocument dokument, Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler) :
            base(dokument, forsendelse, asicEArkiv, databehandler)
        {
        }

        public override XmlElement Xml()
        {
            var body = XmlEnvelope.CreateElement("env", "body", Navnerom.env);
            body.SetAttribute("xmlns:wsu", Navnerom.wsu);
            body.SetAttribute("id", Navnerom.wsu, GuidUtility.BodyId);
            body.AppendChild(StandardBusinessDocumentElement());
            
            return body;
        }

        private XmlElement StandardBusinessDocumentElement()
        {
            var standardBusinessDocument = new StandardBusinessDocument(XmlEnvelope, Forsendelse, AsicEArkiv, Databehandler);
            return standardBusinessDocument.Xml();
        }
    }
}
