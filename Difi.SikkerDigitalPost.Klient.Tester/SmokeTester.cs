using System;
using System.Threading.Tasks;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    [TestClass]
    public class SmokeTester
    {
        [TestMethod]
        public async Task SendDigitalPostIntegrasjonEnkel()
        {
            //Arrange
            var enkelForsendelse = DomeneUtility.GetDigitalForsendelseEnkel();
            var sdpklient = DomeneUtility.GetSikkerDigitalPostKlientQaOffentlig();
            sdpklient.Klientkonfigurasjon.LoggForespørselOgRespons = true;

            //Act
            await SendDokumentpakkeAsync(sdpklient, enkelForsendelse);
            var kvittering = await HentKvitteringOgBekreftAsync(sdpklient, "Enkel Digital Post", enkelForsendelse);

            //Await
            Assert.IsTrue(kvittering is Leveringskvittering, "Klarte ikke hente kvittering eller feilet kvittering");
        }

        [TestMethod]
        public async Task SendDigitalPostIntegrasjonDekkende()
        {
            //Arrange
            var dekkendeDigitalForsendelse = DomeneUtility.GetDigitalForsendelseVarselFlereDokumenterHøyereSikkerhet();
            var sdpklient = DomeneUtility.GetSikkerDigitalPostKlientQaOffentlig();

            //Act
            await SendDokumentpakkeAsync(sdpklient, dekkendeDigitalForsendelse);
            var kvittering = await HentKvitteringOgBekreftAsync(sdpklient, "Dekkende Digital Post", dekkendeDigitalForsendelse);
            Assert.IsTrue(kvittering is Leveringskvittering, "Klarte ikke hente kvittering eller feilet kvittering");
        }

        [TestMethod]
        public async Task SendFysiskPostIntegrasjon()
        {
            //Arrange
            var enkelFysiskForsendelse = DomeneUtility.GetFysiskForsendelseEnkel();
            var sdpklient = DomeneUtility.GetSikkerDigitalPostKlientQaOffentlig();

            //Act
            var transportKvittering = await SendDokumentpakkeAsync(sdpklient, enkelFysiskForsendelse);

            //Assert
            Assert.IsTrue(transportKvittering is TransportOkKvittering);
            var kvittering = await HentKvitteringOgBekreftAsync(sdpklient, "Enkel Fysisk Post", enkelFysiskForsendelse);
            Assert.IsTrue(kvittering is Mottakskvittering, "Klarte ikke hente kvittering eller feilet kvittering");
        }

        [TestMethod]
        public async Task SendDigitaltPåVegneAvIntegrasjon()
        {
            //Arrange
            const string testDepartementetAvsenderOrgnummer = "987656789";
            const string postenDatabehandlerOrgnummer = "984661185";
            var avsender = new Avsender(testDepartementetAvsenderOrgnummer);

            var databehandler = new Databehandler(postenDatabehandlerOrgnummer, DomeneUtility.GetAvsenderSertifikat());
            var forsendelse = new Forsendelse(avsender, DomeneUtility.GetDigitalPostInfoEnkel(), DomeneUtility.GetDokumentpakkeUtenVedlegg(), Prioritet.Normal, Guid.NewGuid().ToString());
            var klientKonfig = new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø);
            
            //Act
            var sdpKlient = new SikkerDigitalPostKlient(databehandler, klientKonfig);

            var transportkvittering = await sdpKlient.SendAsync(forsendelse, true);

            //Assert
            Assert.IsNotNull(transportkvittering);
            var kvittering = await HentKvitteringOgBekreftAsync(sdpKlient, "Send digital paa vegne av", forsendelse);
            Assert.IsTrue(kvittering is Leveringskvittering, "Klarte ikke hente kvittering eller feilet kvittering");
        }

        private async Task<Transportkvittering> SendDokumentpakkeAsync(SikkerDigitalPostKlient sikkerDigitalPostKlient, Forsendelse forsendelse)
        {
            return await sikkerDigitalPostKlient.SendAsync(forsendelse);
        }

        private static async Task<Kvittering> HentKvitteringOgBekreftAsync(SikkerDigitalPostKlient sdpKlient, string testBeskrivelse,
            Forsendelse forsendelse)
        {
            const int hentKvitteringMaksAntallGanger = 10;
            var hentKvitteringPåNytt = true;
            var prøvdPåNytt = 0;

            Kvittering kvittering = null;
            while (hentKvitteringPåNytt && (prøvdPåNytt++ <= hentKvitteringMaksAntallGanger))
            {
                await Task.Delay(2000);
                var kvitteringsforespørsel = new Kvitteringsforespørsel(forsendelse.Prioritet, forsendelse.MpcId);
                kvittering = await sdpKlient.HentKvitteringAsync(kvitteringsforespørsel);

                if (kvittering is TomKøKvittering)
                {
                    continue;
                }

                hentKvitteringPåNytt = false;

                await sdpKlient.BekreftAsync((Forretningskvittering) kvittering);

                var konversasjonsId = HentKonversasjonsIdFraKvittering(kvittering);
                if (konversasjonsId != forsendelse.KonversasjonsId)
                {
                    throw new FieldAccessException(
                        $"Fikk ikke til å hente kvittering for test '{testBeskrivelse}' -- det ble hentet feil kvittering eller ingen kvittering. Var du for rask å hente, " +
                        "eller har noe skjedd galt med hvilken kø du henter fra?");
                }
            }
            return kvittering;
        }

        private static Guid HentKonversasjonsIdFraKvittering(Kvittering kvittering)
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
    }
}