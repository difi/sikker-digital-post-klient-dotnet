using System.Xml;
using SikkerDigitalPost.Klient.Envelope.Abstract;

namespace SikkerDigitalPost.Klient.Envelope.Forretningsmelding
{
    internal class ForretningsmeldingBody : EnvelopeXmlPart
    {
        public ForretningsmeldingBody(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            var body = Context.CreateElement("env", "Body", Navnerom.env);
            body.SetAttribute("xmlns:wsu", Navnerom.wsu);
            body.SetAttribute("Id", Navnerom.wsu, Settings.GuidHandler.BodyId);
            body.AppendChild(Context.ImportNode(StandardBusinessDocumentElement(), true));
            return body;
        }

        private XmlNode StandardBusinessDocumentElement()
        {
            XmlDocument sbdContext = new XmlDocument();
            sbdContext.PreserveWhitespace = true;
            var standardBusinessDocument = new StandardBusinessDocument(Settings, sbdContext);
            return standardBusinessDocument.Xml();
        }
    }
}
