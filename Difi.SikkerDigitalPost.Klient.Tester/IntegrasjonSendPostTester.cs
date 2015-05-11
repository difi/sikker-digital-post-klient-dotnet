using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using ApiClientShared;
using ApiClientShared.Enums;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{

    [TestClass]
    public class IntegrasjonSendPostTester
    {
        private const string OrgnummerPosten = "984661185";
        private const string MottakersertifikatThumbprint =  "B43CAAA0FBEE6C8DA85B47D1E5B7BCAB42AB9ADD";
        readonly ResourceUtility _resourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.testdata");
        public TestContext TestContext { get; set; }
       
        private string _mpcId;
        private Avsender _avsender;

        [TestInitialize]
        public void Initialize()
        {
            _mpcId = "Integrasjonstest-" + Guid.NewGuid();
            
            _avsender = new Avsender(OrgnummerPosten);
            _avsender.Avsenderidentifikator = "digipost";
        }

        
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", @"|DataDirectory|\testdata\integrasjon\digitalpost.csv", "digitalpost#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SendDigitalPostIntegrasjon()
        {
            var beskrivelse = "Ingen beskrivelse";
            try
            {
                //Arrange
                beskrivelse = GetTestContextColumnData("beskrivelse");
                Debug.WriteLine("Kjører test: " + beskrivelse);
                var personnummer = "00000000000";
                var postkasseadresse = GetTestContextColumnData("postkasseadresse");
   
                //Act
                var mottaker = new DigitalPostMottaker(personnummer, postkasseadresse,  CertificateUtility.ReceiverCertificate(MottakersertifikatThumbprint,Language.Norwegian), OrgnummerPosten);
                var postinfo = new DigitalPostInfo(mottaker, "Ikkesensitiv tittel fra Endetester", Sikkerhetsnivå.Nivå3);
               
                //Assert
                SikkerDigitalPostKlient sdpKlient;
                var forsendelse = ByggDokumentpakkeOgSend(_avsender, postinfo, _mpcId, out sdpKlient);
                HentKvitteringOgBekreft(sdpKlient, beskrivelse, _mpcId, forsendelse);
            }
            catch (Exception e)
            {
                Assert.Fail("Feilet for '{0}', feilmelding: {1}, {2}", beskrivelse, e.Message, e.StackTrace);
            }

        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", @"|DataDirectory|\testdata\integrasjon\fysiskpost.csv", "fysiskpost#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SendFysiskPostIntegrasjon()
        {
            var beskrivelse = "Ingen beskrivelse";
            try
            {
                //Arrange
                beskrivelse = GetTestContextColumnData("beskrivelse");
                Debug.WriteLine("Kjører test: " + beskrivelse);
                var mottakernavn = "Ano Nym";
                var mottakerAdresse = new NorskAdresse("1337", "Sandvika") { Adresselinje1 = "Peppern Gror 32" };
                var returAdresse = new NorskAdresse("4242", "Returveien") { Adresselinje1 = "Veien til Nav 43" };
                var posttype = (Posttype)Enum.Parse(typeof(Posttype), GetTestContextColumnData("Posttype"), ignoreCase: true);
                var utskriftsfarge = (Utskriftsfarge)Enum.Parse(typeof(Utskriftsfarge), GetTestContextColumnData("utskriftsfarge"), ignoreCase: true);
                var posthåndtering = (Posthåndtering)Enum.Parse(typeof(Posthåndtering), GetTestContextColumnData("posthandtering"), ignoreCase: true);

                //Act
                var mottaker = new FysiskPostMottaker(mottakernavn, mottakerAdresse, CertificateUtility.ReceiverCertificate(MottakersertifikatThumbprint,Language.Norwegian), OrgnummerPosten);
                var returmottaker = new FysiskPostMottaker("Returkongen", returAdresse);
                var postinfo = new FysiskPostInfo(mottaker, posttype, utskriftsfarge, posthåndtering, returmottaker);
                
                //Assert
                SikkerDigitalPostKlient sdpKlient;
                var forsendelse = ByggDokumentpakkeOgSend(_avsender, postinfo, _mpcId, out sdpKlient);
                HentKvitteringOgBekreft(sdpKlient, beskrivelse, _mpcId, forsendelse);
            }
            catch (Exception e)
            {
                Assert.Fail("Feilet for '{0}', feilmelding: {1}, {2}", beskrivelse, e.Message, e.StackTrace);
            }
        }

        private Forsendelse ByggDokumentpakkeOgSend(Avsender avsender,
            PostInfo postinfo, string mpcId, out SikkerDigitalPostKlient sdpKlient)
        {
            //Arrange
            var hoveddoktype = GetTestContextColumnData("hoveddokumenttype");
            var hoveddokumentsti = GetFirstFile(GetTestContextColumnData("hoveddokument"));
            var vedlegg1Type = GetTestContextColumnData("vedlegg1type");
            var vedlegg1Sti = GetFirstFile(GetTestContextColumnData("vedlegg1"));
            var vedlegg2Type = GetTestContextColumnData("vedlegg2type");
            var vedlegg2Sti = GetFirstFile(GetTestContextColumnData("vedlegg2"));

            //Act
            var databehandler = new Databehandler(OrgnummerPosten,
                CertificateUtility.SenderCertificate("8702F5E55217EC88CF2CCBADAC290BB4312594AC", Language.Norwegian));
            var dokumentpakke = GetDokumentpakke(hoveddokumentsti, hoveddoktype, vedlegg1Sti, vedlegg1Type, vedlegg2Sti,
                vedlegg2Type);

            sdpKlient = new SikkerDigitalPostKlient(databehandler, new Klientkonfigurasjon
            {
                MeldingsformidlerUrl = new Uri("https://qaoffentlig.meldingsformidler.digipost.no/api/ebms")
            });

            //Assert
            var forsendelse = new Forsendelse(avsender, postinfo, dokumentpakke, Prioritet.Prioritert, mpcId);
            var transportkvittering = sdpKlient.Send(forsendelse);

            if (transportkvittering.GetType() == typeof (TransportFeiletKvittering))
            {
                var feilmelding = ((TransportFeiletKvittering) transportkvittering).Beskrivelse;
                Assert.Fail(message: feilmelding);
            }
            
            return forsendelse;
        }

        private void HentKvitteringOgBekreft(SikkerDigitalPostKlient sdpKlient, string testBeskrivelse, string mpcId,
            Forsendelse forsendelse)
        {
            bool hentKvitteringPåNytt = true;

            while (hentKvitteringPåNytt)
            {
                Thread.Sleep(1000);
                var kvitteringsforespørsel = new Kvitteringsforespørsel(Prioritet.Prioritert, mpcId);
                var kvittering = sdpKlient.HentKvittering(kvitteringsforespørsel);

                if (kvittering == null) { continue; }
                
                sdpKlient.Bekreft((Forretningskvittering)kvittering);

                hentKvitteringPåNytt = false;

                Guid konversasjonsId = Guid.Empty;

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

                if (kvittering is Mottakskvittering)
                {
                    var mottakskvittering = (Mottakskvittering)kvittering;
                    konversasjonsId = mottakskvittering.KonversasjonsId;
                }

                if (konversasjonsId.ToString() != forsendelse.KonversasjonsId.ToString())
                {
                    throw new FieldAccessException(
                        String.Format(
                            "Fikk ikke til å hente kvittering for test '{0}' -- det ble hentet feil kvittering eller ingen kvittering. Var du for rask å hente, " +
                            "eller har noe skjedd galt med hvilken kø du henter fra?", testBeskrivelse));
                }                
            }
        }

        private string GetFirstFile(string path)
        {
            var elements = _resourceUtility.GetFiles(path).ToList();

            if (!elements.Any())
            {
                throw new FileNotFoundException(String.Format("Kunne ikke finne filen med ressurssti {0}, har du lagt den i 'testdata' og valgt BuildAction = 'Embedded Resource'? " +
                                                              "Husk at stiseparator SKAL være punktum. Dette er ikke en feil.", path));
            }

            return elements.ElementAt(0);
        }

        private Dokumentpakke GetDokumentpakke(string hoveddokumentsti, string hoveddoktype, string vedlegg1Sti,
            string vedlegg1Type, string vedlegg2Sti, string vedlegg2Type)
        {
            var hoveddokumentBytes = _resourceUtility.ReadAllBytes(false, hoveddokumentsti);
            var hoveddokument = new Dokument(DateTime.Now.ToString("G") + " - Hoveddokument", hoveddokumentBytes,
                hoveddoktype, "NO", "filnavn");
            var dokumentpakke = new Dokumentpakke(hoveddokument);

            LeggVedleggTilDokumentpakke(dokumentpakke, "Vedlegg1Tittel", vedlegg1Sti, vedlegg1Type, "NO", "Vedlegg1Navn");
            LeggVedleggTilDokumentpakke(dokumentpakke, "Vedlegg2Tittel", vedlegg2Sti, vedlegg2Type, "NO", "Vedlegg2Navn");

            return dokumentpakke;
        }

        private void LeggVedleggTilDokumentpakke(Dokumentpakke dokumentpakke, string tittel, string sti, string innholdstype, string språkkode, string filnavn = "null")
        {
            if (String.IsNullOrEmpty(sti) || String.IsNullOrEmpty(innholdstype))
                return;

            var bytes = _resourceUtility.ReadAllBytes(false, sti);
            var vedlegg = new Dokument(tittel, bytes, innholdstype, språkkode, filnavn);
            dokumentpakke.LeggTilVedlegg(vedlegg);
        }

        private string GetTestContextColumnData(string column)
        {
            return TestContext.DataRow[column].ToString();
        }
    }
}
