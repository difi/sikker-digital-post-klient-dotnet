using System.Xml;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Abstract
{
    internal abstract class EnvelopeXmlPart
    {
        protected readonly XmlDocument Context;
        protected readonly EnvelopeSettings Settings;

        protected EnvelopeXmlPart(EnvelopeSettings settings, XmlDocument context)
        {
            Settings = settings;
            Context = context;
        }

        public abstract XmlNode Xml();
    }
}