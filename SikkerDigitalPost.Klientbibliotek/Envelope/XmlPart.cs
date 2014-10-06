using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;

namespace SikkerDigitalPost.Klient.Envelope
{
    internal abstract class XmlPart
    {
        protected readonly XmlDocument XmlEnvelope;
        protected readonly Forsendelse Forsendelse;
        protected readonly AsicEArkiv AsicEArkiv;
        protected readonly Databehandler Databehandler;

        protected XmlPart(XmlDocument xmlEnvelope, Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler)
        {
            XmlEnvelope = xmlEnvelope;
            Forsendelse = forsendelse;
            AsicEArkiv = asicEArkiv;
            Databehandler = databehandler;
        }

        public abstract XmlElement Xml();

    }
}
