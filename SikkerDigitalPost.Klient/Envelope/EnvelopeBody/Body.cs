using System.Xml;

namespace SikkerDigitalPost.Klient.Envelope.EnvelopeBody
{
    internal class Body : XmlPart
    {
        public Body(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            var body = Context.CreateElement("env", "Body", Navnerom.env);
            body.SetAttribute("xmlns:wsu", Navnerom.wsu);
            body.SetAttribute("Id", Navnerom.wsu, Settings.GuidHandler.BodyId);
            body.AppendChild(StandardBusinessDocumentElement());
            
            return body;
        }

        private XmlNode StandardBusinessDocumentElement()
        {
            var standardBusinessDocument = new StandardBusinessDocument(Settings, Context);
            return standardBusinessDocument.Xml();
        }
    }
}
