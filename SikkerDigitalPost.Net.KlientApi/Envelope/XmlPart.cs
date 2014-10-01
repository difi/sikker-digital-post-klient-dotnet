using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.KlientApi.Envelope
{
    public abstract class XmlPart
    {
        protected readonly XmlDocument XmlDocument;
        protected readonly Forsendelse Forsendelse;
        protected readonly Arkiv Arkiv;
        protected readonly Databehandler Databehandler;

        protected XmlPart(XmlDocument xmlDocument, Forsendelse forsendelse, Arkiv arkiv, Databehandler databehandler)
        {
            XmlDocument = xmlDocument;
            Forsendelse = forsendelse;
            Arkiv = arkiv;
            Databehandler = databehandler;
        }

        public abstract XmlElement Xml();

    }
}
