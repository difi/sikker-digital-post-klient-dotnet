using System;
using System.Text;
using System.Threading;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    internal class SmokeTestsHelper
    {
        private static readonly Organisasjonsnummer DifiOrganisasjonsnummer = new Organisasjonsnummer("991825827");
        private readonly SikkerDigitalPostKlient _klient;

        private Forsendelse _forsendelse;
        private Transportkvittering _transportkvittering;
        private Forretningskvittering _forretningskvittering;

        public SmokeTestsHelper(Miljø miljø, bool loggHttp = false, bool brukProxy = false)
        {
            var config = new Klientkonfigurasjon(miljø);

            config.LoggForespørselOgRespons = loggHttp;
            
            if (brukProxy)
            {
                config.ProxyHost = "127.0.0.1";
                config.ProxyPort = 8888;
                config.ProxyScheme = "http";
                config.TimeoutIMillisekunder = 2000;
            }

            var serviceProvider = LoggingUtility.CreateServiceProviderAndSetUpLogging();
            _klient = new SikkerDigitalPostKlient(new Databehandler(DomainUtility.Organisasjonsnummer(), DomainUtility.GetAvsenderCertificate()), config, serviceProvider.GetService<ILoggerFactory>());
        }

        public SmokeTestsHelper Create_Digital_Forsendelse_with_Datatype()
        {
            _forsendelse = DomainUtility.GetForsendelseSimple();
            
            var raw = "<?xml version=\"1.0\" encoding=\"utf-8\"?><lenke xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://begrep.difi.no/sdp/utvidelser/lenke\"><url>https://www.test.no</url><beskrivelse lang=\"nb\">Dette er en lenke utvidelse</beskrivelse></lenke>";

            MetadataDocument metadataDocument = new MetadataDocument("lenke.xml", "application/vnd.difi.dpi.lenke+xml", raw);

            _forsendelse.MetadataDocument = metadataDocument;
            
            return this;
        }
        
        public SmokeTestsHelper Create_Digital_Forsendelse_with_multiple_documents()
        {
            _forsendelse = DomainUtility.GetDigitalDigitalPostWithNotificationMultipleDocumentsAndHigherSecurity(3);
        
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
            _forsendelse.Avsender = new Avsender(new Organisasjonsnummer("984661185")) {Avsenderidentifikator = "digipost"};

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

        public SmokeTestsHelper Expect_Receipt_To_Be(Type forretningskvitteringType)
        {
            Assert_state(_transportkvittering);
            if (_forretningskvittering.GetType() == forretningskvitteringType) return this;
            
            var feilmelding = (Feilmelding) _forretningskvittering;
            throw new Exception($"Fikk tilbake noe annet enn en forventet '{forretningskvitteringType}': {feilmelding}");
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