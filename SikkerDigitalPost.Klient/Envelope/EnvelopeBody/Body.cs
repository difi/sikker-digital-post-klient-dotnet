using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klient.Utilities;

namespace SikkerDigitalPost.Klient.Envelope.EnvelopeBody
{

    internal class Body : XmlPart
    {
        public Body(Envelope rot) : base(rot)
        {
        }

        public override XmlElement Xml()
        {
            var body = Rot.EnvelopeXml.CreateElement("env", "body", Navnerom.env);
            body.SetAttribute("xmlns:wsu", Navnerom.wsu);
            body.SetAttribute("id", Navnerom.wsu, Rot.GuidUtility.BodyId);
            body.AppendChild(StandardBusinessDocumentElement());
            
            return body;
        }

        private XmlElement StandardBusinessDocumentElement()
        {
            var standardBusinessDocument = new StandardBusinessDocument(Rot);
            return standardBusinessDocument.Xml();
        }
    }
}
