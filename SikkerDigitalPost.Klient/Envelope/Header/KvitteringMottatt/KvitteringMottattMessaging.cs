using System.Xml;
using SikkerDigitalPost.Klient.Envelope.Abstract;

namespace SikkerDigitalPost.Klient.Envelope.Header.KvitteringMottatt
{
    internal class KvitteringMottattMessaging : XmlPart
    {
        public KvitteringMottattMessaging(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            return null;
        }
    }
}
