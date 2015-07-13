using System;
using System.Threading;
using System.Threading.Tasks;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{

    [TestClass]
    public class IntegrasjonSendPostTester
    {
        private int _hentKvitteringerMaksAntallGanger = 10;

        [TestMethod]
        public void SendDigitalPostIntegrasjonEnkel()
        {
            try
            {
                //Arrange
                var enkelForsendelse = DomeneUtility.GetDigitalForsendelseEnkel();
                var sdpklient = DomeneUtility.GetSikkerDigitalPostKlientQaOffentlig();
                
                //Act
                SendDokumentpakke(sdpklient, enkelForsendelse);
                var antallGangerForsøkt = HentKvitteringOgBekreft(sdpklient, "Enkel Digital Post", enkelForsendelse.MpcId, enkelForsendelse).Result;
                Assert.IsTrue(antallGangerForsøkt >= _hentKvitteringerMaksAntallGanger, "Fant ikke kvittering innen maksimalt antall forsøk.");
            }
            catch (Exception e)
            {
                //Assert
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void SendDigitalPostIntegrasjonDekkende()
        {
            try
            {
                //Arrange
                var dekkendeDigitalForsendelse = DomeneUtility.GetDigitalForsendelseVarselFlereDokumenterHøyereSikkerhet();
                var sdpklient = DomeneUtility.GetSikkerDigitalPostKlientQaOffentlig();

                //Act
                SendDokumentpakke(sdpklient, dekkendeDigitalForsendelse);
                var antallGangerForsøkt = HentKvitteringOgBekreft(sdpklient, "Dekkende Digital Post", dekkendeDigitalForsendelse.MpcId, dekkendeDigitalForsendelse).Result;
                Assert.IsTrue(antallGangerForsøkt >= _hentKvitteringerMaksAntallGanger, "Fant ikke kvittering innen maksimalt antall forsøk.");
            }
            catch (Exception e)
            {
                //Assert
                Assert.Fail(e.Message);
            }

        }

        [TestMethod]
        public void SendFysiskPostIntegrasjon()
        {
            try
            {
                //Arrange
                var enkelFysiskForsendelse = DomeneUtility.GetFysiskForsendelseEnkel();
                var sdpklient = DomeneUtility.GetSikkerDigitalPostKlientQaOffentlig();

                //Act
                SendDokumentpakke(sdpklient, enkelFysiskForsendelse);
                var antallGangerForsøkt = HentKvitteringOgBekreft(sdpklient, "Enkel Fysisk Post",enkelFysiskForsendelse.MpcId, enkelFysiskForsendelse).Result;
                Assert.IsTrue(antallGangerForsøkt >= _hentKvitteringerMaksAntallGanger,"Fant ikke kvittering innen maksimalt antall forsøk.");
            }
            catch (Exception e)
            {
                //Assert
                Assert.Fail(e.Message);
            }
        }

        private void SendDokumentpakke(SikkerDigitalPostKlient sikkerDigitalPostKlient, Forsendelse forsendelse)
        {
            var transportkvittering = sikkerDigitalPostKlient.Send(forsendelse);

            if (transportkvittering.GetType() == typeof(TransportFeiletKvittering))
            {
                var feilmelding = ((TransportFeiletKvittering)transportkvittering).Beskrivelse;
                Assert.Fail(feilmelding);
            }
        }

        private async Task<int> HentKvitteringOgBekreft(SikkerDigitalPostKlient sdpKlient, string testBeskrivelse, string mpcId,
            Forsendelse forsendelse)
        {
            var hentKvitteringPåNytt = true;
            var prøvdPåNytt = 0;
            
            while (hentKvitteringPåNytt && (prøvdPåNytt++ <= _hentKvitteringerMaksAntallGanger))
            {
                Thread.Sleep(1000);
                var kvitteringsforespørsel = new Kvitteringsforespørsel(Prioritet.Prioritert, mpcId);
                var kvittering = await sdpKlient.HentKvitteringAsync(kvitteringsforespørsel);

                if (kvittering == null) { continue; }

                sdpKlient.Bekreft((Forretningskvittering)kvittering);

                hentKvitteringPåNytt = false;

                var konversasjonsId = Guid.Empty;

                if (kvittering is Feilmelding)
                {
                    var feilmelding = (Feilmelding)kvittering;
                    konversasjonsId = feilmelding.KonversasjonsId;
                    Assert.Fail("Test '{0}' feilet. Feilmelding fra Meldingsformidler: {1}",
                        testBeskrivelse,
                        feilmelding.Detaljer);
                    
                }

                if (kvittering is Leveringskvittering)
                {
                    var leveringskvittering = (Leveringskvittering)kvittering;
                    konversasjonsId = leveringskvittering.KonversasjonsId;
                }

               
                if (konversasjonsId.ToString() != forsendelse.KonversasjonsId.ToString())
                {
                    throw new FieldAccessException(
                        string.Format(
                            "Fikk ikke til å hente kvittering for test '{0}' -- det ble hentet feil kvittering eller ingen kvittering. Var du for rask å hente, " +
                            "eller har noe skjedd galt med hvilken kø du henter fra?", testBeskrivelse));
                }
            }
            return prøvdPåNytt;
        }

    }
}
