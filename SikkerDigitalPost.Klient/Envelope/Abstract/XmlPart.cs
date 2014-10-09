using System.Xml;

namespace SikkerDigitalPost.Klient.Envelope.Abstract
{
    internal abstract class XmlPart
    {
        protected readonly EnvelopeSettings Settings;
        protected readonly XmlDocument Context;

        protected XmlPart(EnvelopeSettings settings, XmlDocument context)
        {
            Settings = settings;
            Context = context;
        }

        public abstract XmlNode Xml();
    }
}
