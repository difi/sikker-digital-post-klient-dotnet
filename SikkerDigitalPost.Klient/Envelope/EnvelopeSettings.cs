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
        public readonly Leveringskvittering Leveringskvittering;

        /// <summary>
        /// Settings for KvitteringsEnvelope
        /// </summary>
        public EnvelopeSettings(Kvitteringsforespørsel kvitteringsforespørsel, Databehandler databehandler, GuidHandler guidHandler)
        {
            Kvitteringsforespørsel = kvitteringsforespørsel;
            Databehandler = databehandler;
            GuidHandler = guidHandler;
        }

        /// <summary>
        /// Settings for DigitalPostForsendelse
        /// </summary>
        public EnvelopeSettings(Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler, GuidHandler guidHandler)
        {
            Forsendelse = forsendelse;
            AsicEArkiv = asicEArkiv;
            Databehandler = databehandler;
            GuidHandler = guidHandler;
        }
        
        /// <summary>
        /// Settings for BekreftKvittering
        /// </summary>
        public EnvelopeSettings(Leveringskvittering leveringskvittering, Databehandler databehandler, GuidHandler guidHandler)
        {
            Leveringskvittering = leveringskvittering;
            Databehandler = databehandler;
            GuidHandler = guidHandler;
        }
    }
}
