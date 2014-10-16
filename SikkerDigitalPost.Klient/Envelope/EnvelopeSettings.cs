using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klient.AsicE;

namespace SikkerDigitalPost.Klient.Envelope
{
    internal class EnvelopeSettings
    {
        public readonly Forsendelse Forsendelse;
        internal readonly AsicEArkiv AsicEArkiv;
        public readonly Databehandler Databehandler;
        internal readonly GuidHandler GuidHandler;
        public readonly Kvitteringsforespørsel Kvitteringsforespørsel;
        public readonly Forretningskvittering ForrigeKvittering;

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
        public EnvelopeSettings(Forretningskvittering forrigeKvittering, Databehandler databehandler, GuidHandler guidHandler)
        {
            ForrigeKvittering = forrigeKvittering;
            Databehandler = databehandler;
            GuidHandler = guidHandler;
        }
    }
}
