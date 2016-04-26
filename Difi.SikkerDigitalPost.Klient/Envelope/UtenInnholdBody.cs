using System.Xml;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope
{
    internal class UtenInnholdBody : EnvelopeXmlPart
    {
        public UtenInnholdBody(EnvelopeSettings settings, XmlDocument context)
            : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            var body = Context.CreateElement("env", "Body", NavneromUtility.SoapEnvelopeEnv12);
            body.SetAttribute("xmlns:wsu", NavneromUtility.WssecurityUtility10);
            body.SetAttribute("Id", NavneromUtility.WssecurityUtility10, Settings.GuidUtility.BodyId);
            return body;
        }
    }
}