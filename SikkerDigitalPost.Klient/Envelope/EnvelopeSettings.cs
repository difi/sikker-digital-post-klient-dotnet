using System.Security.Cryptography.Pkcs;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer;
using SikkerDigitalPost.Domene.Entiteter.Post;

namespace SikkerDigitalPost.Klient.Envelope
{
    internal class EnvelopeSettings
    {
        public readonly Forsendelse Forsendelse;
        internal readonly AsicEArkiv AsicEArkiv;
        public readonly Databehandler Databehandler;
        internal readonly GuidHandler GuidHandler;
        public readonly Kvitteringsforespørsel Kvitteringsforespørsel;

        public EnvelopeSettings(Kvitteringsforespørsel kvitteringsforespørsel, GuidHandler guidHandler)
        {
            Kvitteringsforespørsel = kvitteringsforespørsel;
            GuidHandler = guidHandler;
        }

        public EnvelopeSettings(Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler, GuidHandler guidHandler)
        {
            Forsendelse = forsendelse;
            AsicEArkiv = asicEArkiv;
            Databehandler = databehandler;
            GuidHandler = guidHandler;
        }
    }
}
