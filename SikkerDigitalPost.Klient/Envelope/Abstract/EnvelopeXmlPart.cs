using System.Xml;

namespace SikkerDigitalPost.Klient.Envelope.Abstract
{
    internal abstract class EnvelopeXmlPart
    {
        protected readonly EnvelopeSettings Settings;
        protected readonly XmlDocument Context;

        protected EnvelopeXmlPart(EnvelopeSettings settings, XmlDocument context)
        {
            Settings = settings;
            Context = context;
        }

        public abstract XmlNode Xml();
    }
}
