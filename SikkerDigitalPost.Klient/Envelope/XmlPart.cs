using System.Xml;

namespace SikkerDigitalPost.Klient.Envelope
{
    internal abstract class XmlPart
    {
        /// <summary>
        /// Inneholder objekter med verdier som skal brukes i Envelope xml.
        /// </summary>
        protected readonly EnvelopeSettings Settings;

        /// <summary>
        /// Definerer konteksten (XmlDocument) nye noder lages i.
        /// </summary>
        protected readonly XmlDocument Context;

        /// <param name="settings">Inneholder objekter med verdier som skal brukes i Envelope xml.</param>
        /// <param name="context">Definerer konteksten (XmlDokumentet) nye noder lages i.</param>
        protected XmlPart(EnvelopeSettings settings, XmlDocument context)
        {
            Settings = settings;
            Context = context;
        }

        public abstract XmlNode Xml();
    }
}
