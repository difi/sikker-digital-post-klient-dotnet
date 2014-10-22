using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klient.AsicE;
using KvitteringsForespørsel = SikkerDigitalPost.Domene.Entiteter.Kvitteringer.Kvitteringsforespørsel;

namespace SikkerDigitalPost.Klient.Envelope
{
    internal class EnvelopeSettings
    {
        public Forsendelse Forsendelse { get; private set; }
        public Databehandler Databehandler { get; private set; }
        public Domene.Entiteter.Kvitteringer.Kvitteringsforespørsel Kvitteringsforespørsel { get; private set; }
        public Forretningskvittering ForrigeKvittering { get; private set; }
        internal AsicEArkiv AsicEArkiv { get; private set; }
        internal GuidHandler GuidHandler { get; private set; }
        internal Klientkonfigurasjon Konfigurasjon { get; private set; }
        /// <summary>
        /// Settings for KvitteringsEnvelope
        /// </summary>
        public EnvelopeSettings(Domene.Entiteter.Kvitteringer.Kvitteringsforespørsel kvitteringsforespørsel, Databehandler databehandler, GuidHandler guidHandler)
        {
            Kvitteringsforespørsel = kvitteringsforespørsel;
            Databehandler = databehandler;
            GuidHandler = guidHandler;
        }

        /// <summary>
        /// Settings for DigitalPostForsendelse
        /// </summary>
        public EnvelopeSettings(Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler, GuidHandler guidHandler, Klientkonfigurasjon konfigurasjon)
        {
            Forsendelse = forsendelse;
            AsicEArkiv = asicEArkiv;
            Databehandler = databehandler;
            GuidHandler = guidHandler;
            Konfigurasjon = konfigurasjon;
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
