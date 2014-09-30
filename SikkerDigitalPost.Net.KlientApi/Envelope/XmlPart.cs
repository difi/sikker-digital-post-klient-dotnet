using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.KlientApi.Envelope
{
    public abstract class XmlPart
    {
        protected readonly XmlDocument XmlDocument;
        protected readonly Forsendelse Forsendelse;

        protected XmlPart(XmlDocument xmlDocument, Forsendelse forsendelse)
        {
            XmlDocument = xmlDocument;
            Forsendelse = forsendelse;
        }

        public abstract XmlElement Xml();

    }
}
