using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.KlientApi.Envelope
{
    public abstract class XmlPart
    {
        protected readonly XmlDocument XmlDocument;
        protected readonly Forsendelse Forsendelse;
        protected readonly AsicEArkiv AsicEArkiv;
        protected readonly Databehandler Databehandler;

        protected XmlPart(XmlDocument xmlDocument, Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler)
        {
            XmlDocument = xmlDocument;
            Forsendelse = forsendelse;
            AsicEArkiv = asicEArkiv;
            Databehandler = databehandler;
        }

        public abstract XmlElement Xml();

    }
}
