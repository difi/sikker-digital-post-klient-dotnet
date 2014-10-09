using System.Xml;
using SikkerDigitalPost.Klient.Envelope.Abstract;

namespace SikkerDigitalPost.Klient.Envelope.Header.Kvittering
{
    internal class KvitteringsHeader : AbstractHeader
    {
        public KvitteringsHeader(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            throw new System.NotImplementedException();
        }

        protected override XmlNode SecurityElement()
        {
            throw new System.NotImplementedException();
        }

        protected override XmlNode MessagingElement()
        {
            throw new System.NotImplementedException();
        }
    }
}
