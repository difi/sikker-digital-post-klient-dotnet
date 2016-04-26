using System.Xml;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding
{
    internal class ForretningsmeldingBody : EnvelopeXmlPart
    {
        public ForretningsmeldingBody(EnvelopeSettings settings, XmlDocument context)
            : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            var body = Context.CreateElement("env", "Body", NavneromUtility.SoapEnvelopeEnv12);
            body.SetAttribute("xmlns:wsu", NavneromUtility.WssecurityUtility10);
            body.SetAttribute("Id", NavneromUtility.WssecurityUtility10, Settings.GuidUtility.BodyId);
            body.AppendChild(Context.ImportNode(StandardBusinessDocumentElement(), true));
            return body;
        }

        private XmlNode StandardBusinessDocumentElement()
        {
            var sbdContext = new XmlDocument {PreserveWhitespace = true};
            var standardBusinessDocument = new StandardBusinessDocument(Settings, sbdContext);
            return standardBusinessDocument.Xml();
        }
    }
}