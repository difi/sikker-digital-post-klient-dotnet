using Difi.SikkerDigitalPost.Klient.AsicE;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Utilities;
using KvitteringsForespørsel = Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Kvitteringsforespørsel;

namespace Difi.SikkerDigitalPost.Klient.Envelope
{
    internal class EnvelopeSettings
    {
        /// <summary>
        ///     Settings for KvitteringsEnvelope
        /// </summary>
        public EnvelopeSettings(KvitteringsForespørsel kvitteringsforespørsel, Databehandler databehandler, GuidUtility guidHandler)
        {
            Kvitteringsforespørsel = kvitteringsforespørsel;
            Databehandler = databehandler;
            GuidHandler = guidHandler;
        }

        /// <summary>
        ///     Settings for DigitalPostForsendelse
        /// </summary>
        public EnvelopeSettings(Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler, GuidUtility guidHandler, Klientkonfigurasjon konfigurasjon)
        {
            Forsendelse = forsendelse;
            AsicEArkiv = asicEArkiv;
            Databehandler = databehandler;
            GuidHandler = guidHandler;
            Konfigurasjon = konfigurasjon;
        }

        /// <summary>
        ///     Settings for BekreftKvittering
        /// </summary>
        public EnvelopeSettings(Forretningskvittering forrigeKvittering, Databehandler databehandler, GuidUtility guidHandler)
        {
            ForrigeKvittering = forrigeKvittering;
            Databehandler = databehandler;
            GuidHandler = guidHandler;
        }

        public Forsendelse Forsendelse { get; private set; }
        public Databehandler Databehandler { get; private set; }
        public KvitteringsForespørsel Kvitteringsforespørsel { get; private set; }
        public Forretningskvittering ForrigeKvittering { get; private set; }
        internal AsicEArkiv AsicEArkiv { get; private set; }
        internal GuidUtility GuidHandler { get; private set; }
        internal Klientkonfigurasjon Konfigurasjon { get; private set; }
    }
}