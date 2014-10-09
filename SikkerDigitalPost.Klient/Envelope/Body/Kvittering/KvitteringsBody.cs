using System.Xml;
using SikkerDigitalPost.Klient.Envelope.Abstract;

namespace SikkerDigitalPost.Klient.Envelope.Body.Kvittering
{
    internal class KvitteringsBody : XmlPart
    {
        public KvitteringsBody(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            XmlElement body = Context.CreateElement("env", "Body", Navnerom.env);
            body.SetAttribute("xmlns:wsu", Navnerom.wsu);
            body.SetAttribute("Id", Navnerom.wsu, Settings.GuidHandler.BodyId);
            return body;
        }
    }
}
