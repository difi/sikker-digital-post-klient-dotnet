using System;
using System.Threading;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Tester.Properties;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{

    [TestClass]
    public class SmokeTester
    {      
        [TestMethod]
        public void SendDigitalPostIntegrasjonEnkel()
        {
            //Arrange
            var enkelForsendelse = DomeneUtility.GetDigitalForsendelseEnkel();
            var sdpklient = DomeneUtility.GetSikkerDigitalPostKlientQaOffentlig();

            //Act
            SendDokumentpakke(sdpklient, enkelForsendelse);
            var kvittering = HentKvitteringOgBekreft(sdpklient, "Enkel Digital Post", enkelForsendelse);
            Assert.IsTrue(kvittering is Leveringskvittering, "Klarte ikke hente kvittering eller feilet kvittering");
        }

        [TestMethod]
        public void SendDigitalPostIntegrasjonDekkende()
        {
            //Arrange
            var dekkendeDigitalForsendelse = DomeneUtility.GetDigitalForsendelseVarselFlereDokumenterHøyereSikkerhet();
            var sdpklient = DomeneUtility.GetSikkerDigitalPostKlientQaOffentlig();

            //Act
            SendDokumentpakke(sdpklient, dekkendeDigitalForsendelse);
            var kvittering = HentKvitteringOgBekreft(sdpklient, "Dekkende Digital Post", dekkendeDigitalForsendelse);
            Assert.IsTrue(kvittering is Leveringskvittering, "Klarte ikke hente kvittering eller feilet kvittering");

        }

        [TestMethod]
        public void SendFysiskPostIntegrasjon()
        {
            //Arrange
            var enkelFysiskForsendelse = DomeneUtility.GetFysiskForsendelseEnkel();
            var sdpklient = DomeneUtility.GetSikkerDigitalPostKlientQaOffentlig();

            //Act
            var transportKvittering = SendDokumentpakke(sdpklient, enkelFysiskForsendelse);

            //Assert
            Assert.IsTrue(transportKvittering is TransportOkKvittering);
            var kvittering = HentKvitteringOgBekreft(sdpklient, "Enkel Fysisk Post", enkelFysiskForsendelse);
            Assert.IsTrue(kvittering is Mottakskvittering, "Klarte ikke hente kvittering eller feilet kvittering");
        }

        [TestMethod]
        public void SendDigitaltPåVegneAvIntegrasjon()
        {
            //Arrange
            const string testDepartementetAvsenderOrgnummer = "987656789";
            const string postenDatabehandlerOrgnummer = "984661185";
            var avsender = new Avsender(testDepartementetAvsenderOrgnummer);

            var databehandler = new Databehandler(postenDatabehandlerOrgnummer, DomeneUtility.GetAvsenderSertifikat());
            var forsendelse = new Forsendelse(avsender, DomeneUtility.GetDigitalPostInfoEnkel(), DomeneUtility.GetDokumentpakkeUtenVedlegg(), Prioritet.Normal, Guid.NewGuid().ToString());
            var klientKonfig = new Klientkonfigurasjon(Miljø.Test)
            {
                LoggXmlTilFil = true
            };

            //Act
            var sdpKlient = new SikkerDigitalPostKlient(databehandler, klientKonfig);
            var transportkvittering = sdpKlient.Send(forsendelse, true);


            //Assert
            Assert.IsNotNull(transportkvittering);
            var kvittering = HentKvitteringOgBekreft(sdpKlient, "Send digital paa vegne av", forsendelse);
            Assert.IsTrue(kvittering is Leveringskvittering, "Klarte ikke hente kvittering eller feilet kvittering");
        }

        private Transportkvittering SendDokumentpakke(SikkerDigitalPostKlient sikkerDigitalPostKlient, Forsendelse forsendelse)
        {
            return sikkerDigitalPostKlient.Send(forsendelse);
        }

        private static Kvittering HentKvitteringOgBekreft(SikkerDigitalPostKlient sdpKlient, string testBeskrivelse,
            Forsendelse forsendelse)
        {
            const int hentKvitteringMaksAntallGanger = 10;
            var hentKvitteringPåNytt = true;
            var prøvdPåNytt = 0;

            Kvittering kvittering = null;
            while (hentKvitteringPåNytt && (prøvdPåNytt++ <= hentKvitteringMaksAntallGanger))
            {
                Thread.Sleep(1000);
                var kvitteringsforespørsel = new Kvitteringsforespørsel(forsendelse.Prioritet, forsendelse.MpcId);
                kvittering = sdpKlient.HentKvittering(kvitteringsforespørsel);

                if (kvittering == null) { continue; }
                hentKvitteringPåNytt = false;

                sdpKlient.Bekreft((Forretningskvittering)kvittering);

                var konversasjonsId = HentKonversasjonsIdFraKvittering(kvittering);
                if (konversasjonsId != forsendelse.KonversasjonsId)
                {
                    throw new FieldAccessException(
                        string.Format(
                            "Fikk ikke til å hente kvittering for test '{0}' -- det ble hentet feil kvittering eller ingen kvittering. Var du for rask å hente, " +
                            "eller har noe skjedd galt med hvilken kø du henter fra?", testBeskrivelse));
                }
            }
            return kvittering;
        }

        private static Guid HentKonversasjonsIdFraKvittering(Kvittering kvittering)
        {
            Guid konversasjonsId = Guid.Empty;

            if (kvittering is Feilmelding)
            {
                var feilmelding = (Feilmelding)kvittering;
                konversasjonsId = feilmelding.KonversasjonsId;
            }

            if (kvittering is Leveringskvittering)
            {
                var leveringskvittering = (Leveringskvittering)kvittering;
                konversasjonsId = leveringskvittering.KonversasjonsId;
            }

            if (kvittering is Mottakskvittering)
            {
                var mottakskvittering = (Mottakskvittering)kvittering;
                konversasjonsId = mottakskvittering.KonversasjonsId;
            }

            return konversasjonsId;
        }
    }
}