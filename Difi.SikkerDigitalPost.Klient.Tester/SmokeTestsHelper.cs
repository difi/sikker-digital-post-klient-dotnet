using System;
using System.Threading;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    internal class SmokeTestsHelper
    {
        private const string BringOrganisasjonsnummer = "988015814";
        private const string BringThumbprint = "2d7f30dd05d3b7fc7ae5973a73f849083b2040ed";
        private readonly SikkerDigitalPostKlient _klient;

        private Forsendelse _forsendelse;
        private Transportkvittering _transportkvittering;
        private Forretningskvittering _forretningskvittering;

        public SmokeTestsHelper(Miljø miljø)
        {
            _klient = new SikkerDigitalPostKlient(new Databehandler(BringOrganisasjonsnummer, BringThumbprint), new Klientkonfigurasjon(miljø));
        }

        public SmokeTestsHelper Create_Digital_Forsendelse_with_multiple_documents()
        {
            _forsendelse = DomainUtility.GetDigitalDigitalPostWithNotificationMultipleDocumentsAndHigherSecurity();

            return this;
        }

        public SmokeTestsHelper Create_Physical_Forsendelse()
        {
            _forsendelse = DomainUtility.GetFysiskPostSimple();

            return this;
        }

        public SmokeTestsHelper Create_digital_forsendelse_with_different_sender()
        {
            _forsendelse = DomainUtility.GetForsendelseSimple();
            _forsendelse.Avsender = new Avsender("984661185") {Avsenderidentifikator = "digipost"};

            return this;
        }

        public SmokeTestsHelper Send()
        {
            Assert_state(_forsendelse);

            _transportkvittering = _klient.Send(_forsendelse);

            return this;
        }

        public SmokeTestsHelper Expect_Message_Response_To_Be_TransportOkKvittering()
        {
            Assert_state(_transportkvittering);

            Assert.IsType<TransportOkKvittering>(_transportkvittering);

            return this;
        }

        public SmokeTestsHelper Fetch_Receipt()
        {
            Assert_state(_transportkvittering);

            const int maxTries = 10;
            var kvitteringReceived = false;
            var numberOfTries = 0;

            while (!kvitteringReceived && (numberOfTries++ <= maxTries))
            {
                var kvittering = GetSingleKvittering();

                if (kvittering is TomKøKvittering)
                {
                    continue;
                }

                kvitteringReceived = true;
                _forretningskvittering = (Forretningskvittering) kvittering;
            }

            Assert.True(kvitteringReceived, "Fikk ikke til å hente kvittering. Var du for rask å hente, eller har noe skjedd galt med hvilken kø du henter fra?");

            return this;
        }

        public SmokeTestsHelper ConfirmReceipt()
        {
            Assert_state(_forretningskvittering);

            _klient.Bekreft(_forretningskvittering);

            var konversasjonsId = GetKonversasjonsIdFromKvittering(_forretningskvittering);

            Assert.Equal(_forsendelse.KonversasjonsId, konversasjonsId);

            return this;
        }

        private Kvittering GetSingleKvittering()
        {
            Thread.Sleep(3000);

            var kvitteringsforespørsel = new Kvitteringsforespørsel(_forsendelse.Prioritet, _forsendelse.MpcId);
            var kvittering = _klient.HentKvittering(kvitteringsforespørsel);
            return kvittering;
        }

        private static Guid GetKonversasjonsIdFromKvittering(Kvittering kvittering)
        {
            var konversasjonsId = Guid.Empty;

            if (kvittering is Feilmelding)
            {
                var feilmelding = (Feilmelding) kvittering;
                konversasjonsId = feilmelding.KonversasjonsId;
            }
            else if (kvittering is Leveringskvittering)
            {
                var leveringskvittering = (Leveringskvittering) kvittering;
                konversasjonsId = leveringskvittering.KonversasjonsId;
            }
            else if (kvittering is Mottakskvittering)
            {
                var mottakskvittering = (Mottakskvittering) kvittering;
                konversasjonsId = mottakskvittering.KonversasjonsId;
            }

            return konversasjonsId;
        }

        private static void Assert_state(object obj)
        {
            if (obj == null)
            {
                throw new InvalidOperationException("Requires gradually built state. Make sure you use functions in the correct order.");
            }
        }
    }
}