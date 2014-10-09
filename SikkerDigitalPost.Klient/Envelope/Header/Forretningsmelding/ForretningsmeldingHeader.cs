using System.Xml;
using SikkerDigitalPost.Klient.Envelope.Abstract;

namespace SikkerDigitalPost.Klient.Envelope.Header.Forretningsmelding
{
    internal class Header : AbstractHeader
    {
        private Security _security;

        public Header(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        protected override XmlNode SecurityElement()
        {
            _security = new Security(Settings, Context);
            return _security.Xml();
        }

        protected override XmlNode MessagingElement()
        {
            var messaging = new Messaging(Settings, Context);
            return messaging.Xml();
        }

        public void AddSignatureElement()
        {
            _security.AddSignatureElement();
        }
    }
}
