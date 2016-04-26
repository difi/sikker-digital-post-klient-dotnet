using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Difi.SikkerDigitalPost.Klient.Utilities;
using KvitteringsForespørsel = Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Kvitteringsforespørsel;

namespace Difi.SikkerDigitalPost.Klient.Envelope
{
    internal class EnvelopeSettings
    {
        /// <summary>
        ///     Settings for KvitteringsEnvelope
        /// </summary>
        public EnvelopeSettings(KvitteringsForespørsel kvitteringsforespørsel, Databehandler databehandler, GuidUtility guidUtility)
        {
            Kvitteringsforespørsel = kvitteringsforespørsel;
            Databehandler = databehandler;
            GuidUtility = guidUtility;
        }

        /// <summary>
        ///     Settings for DigitalPostForsendelse
        /// </summary>
        public EnvelopeSettings(Forsendelse forsendelse, DocumentBundle documentBundle, Databehandler databehandler, GuidUtility guidUtility, Klientkonfigurasjon konfigurasjon)
        {
            Forsendelse = forsendelse;
            DocumentBundle = documentBundle;
            Databehandler = databehandler;
            GuidUtility = guidUtility;
            Konfigurasjon = konfigurasjon;
        }

        /// <summary>
        ///     Settings for BekreftKvittering
        /// </summary>
        public EnvelopeSettings(Forretningskvittering forrigeKvittering, Databehandler databehandler, GuidUtility guidUtility)
        {
            ForrigeKvittering = forrigeKvittering;
            Databehandler = databehandler;
            GuidUtility = guidUtility;
        }

        public Forsendelse Forsendelse { get; private set; }

        public Databehandler Databehandler { get; private set; }

        public KvitteringsForespørsel Kvitteringsforespørsel { get; private set; }

        public Forretningskvittering ForrigeKvittering { get; private set; }

        internal DocumentBundle DocumentBundle { get; private set; }

        internal GuidUtility GuidUtility { get; private set; }

        internal Klientkonfigurasjon Konfigurasjon { get; private set; }
    }
}