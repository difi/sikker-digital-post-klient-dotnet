using System.Xml;

namespace SikkerDigitalPost.Klient.Envelope.Body
{
    class KvitteringsBody : XmlPart
    {
        public KvitteringsBody(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            var body = Context.CreateElement("env", "Body", Navnerom.env);
            body.SetAttribute("xmlns:wsu", Navnerom.wsu);
            body.SetAttribute("Id", Navnerom.wsu, Settings.GuidHandler.BodyId);
            return body;
        }
    }
}
