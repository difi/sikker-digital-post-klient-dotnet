using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.FysiskPost;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer.Forretning;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer.Transport;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Domene.Enums;
using SikkerDigitalPost.Klient;
using SikkerDigitalPost.Tester.Utilities;

namespace SikkerDigitalPost.Tester
{

    [TestClass]
    public class SendDigitalPostTester
    {
        private const string OrgnummerPosten = "984661185";
        private const string MottakersertifikatThumbprint =  "B43CAAA0FBEE6C8DA85B47D1E5B7BCAB42AB9ADD";

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", @"|DataDirectory|\testdata\integrasjon\digitalpost.csv", "digitalpost#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SendDigitalPostIntegrasjon()
        {
            var beskrivelse = "Ingen beskrivelse";
            try
            {
                beskrivelse = GetColumnData("beskrivelse");
                Debug.WriteLine("Kjører test: " + beskrivelse);
                var personnummer = "00000000000";
                var postkasseadresse = GetColumnData("postkasseadresse");
   

                var mottaker = new DigitalPostMottaker(personnummer, postkasseadresse, SertifikatUtility.MottakerSertifikat(MottakersertifikatThumbprint), OrgnummerPosten);
                var postInfo = new DigitalPostInfo(mottaker, "Ikkesensitiv tittel fra Endetester", Sikkerhetsnivå.Nivå3, false);
                var behandlingsansvarlig = new Behandlingsansvarlig(OrgnummerPosten);
                behandlingsansvarlig.Avsenderidentifikator = "digipost";
                var mpcId = "digitalpostintegrasjon-" + Guid.NewGuid();
                
                Forsendelse forsendelse;
                var sdpKlient = ByggDokumentpakkeOgSend(behandlingsansvarlig, postInfo, mpcId, out forsendelse);

                HentKvitteringOgBekreft(sdpKlient, beskrivelse, mpcId, forsendelse);
            }
            catch (Exception e)
            {
                Assert.Fail(String.Format("Feilet for '{0}', feilmelding: {1}, {2}", beskrivelse, e.Message, e.StackTrace));
            }

        }

        private SikkerDigitalPostKlient ByggDokumentpakkeOgSend(Behandlingsansvarlig behandlingsansvarlig,
            PostInfo postInfo, string mpcId, out Forsendelse forsendelse)
        {
            var hoveddoktype = GetColumnData("hoveddokumenttype");
            var hoveddokumentsti = GetFirstFile(GetColumnData("hoveddokument"));
            var vedlegg1Type = GetColumnData("vedlegg1type");
            var vedlegg1Sti = GetFirstFile(GetColumnData("vedlegg1"));
            var vedlegg2Type = GetColumnData("vedlegg2type");
            var vedlegg2Sti = GetFirstFile(GetColumnData("vedlegg2"));

            var databehandler = new Databehandler(OrgnummerPosten,
                SertifikatUtility.AvsenderSertifkat("8702F5E55217EC88CF2CCBADAC290BB4312594AC"));
            var dokumentpakke = ByggDokumentpakke(hoveddokumentsti, hoveddoktype, vedlegg1Sti, vedlegg1Type, vedlegg2Sti,
                vedlegg2Type);

            var sdpKlient = new SikkerDigitalPostKlient(databehandler, new Klientkonfigurasjon
            {
                MeldingsformidlerUrl = new Uri("https://qaoffentlig.meldingsformidler.digipost.no/api/ebms")
            });

            forsendelse = new Forsendelse(behandlingsansvarlig, postInfo, dokumentpakke, Prioritet.Prioritert, mpcId);
            var transportkvittering = sdpKlient.Send(forsendelse);

            if (transportkvittering.GetType() == typeof (TransportFeiletKvittering))
            {
                var feil = ((TransportFeiletKvittering) transportkvittering).Beskrivelse;
                Assert.Fail(feil);
            }
            return sdpKlient;
        }
        
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", @"|DataDirectory|\testdata\integrasjon\fysiskpost.csv", "fysiskpost#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SendFysiskPostIntegrasjon()
        {
            var beskrivelse = "Ingen beskrivelse";
            try
            {

                beskrivelse = GetColumnData("beskrivelse");
                Debug.WriteLine("Kjører test: " + beskrivelse);
                var mottakernavn = "Ano Nym";
                var mottakerAdresse = new NorskAdresse("1337", "Sandvika") { Adresselinje1 = "Peppern Gror 32" };
                var returAdresse = new NorskAdresse("4242", "Returveien") { Adresselinje1 = "Veien til Nav 43" };

                var posttype = (Posttype)Enum.Parse(typeof(Posttype), GetColumnData("Posttype"), ignoreCase: true);
                var utskriftsfarge = (Utskriftsfarge)Enum.Parse(typeof(Utskriftsfarge), GetColumnData("utskriftsfarge"), ignoreCase: true);
                var posthåndtering = (Posthåndtering)Enum.Parse(typeof(Posthåndtering), GetColumnData("posthandtering"), ignoreCase: true);

                var mottaker = new FysiskPostMottaker(mottakernavn, mottakerAdresse, SertifikatUtility.MottakerSertifikat(MottakersertifikatThumbprint), OrgnummerPosten);
                var returmottaker = new FysiskPostMottaker("Returkongen", returAdresse);
                var postinfo = new FysiskPostInfo(mottaker, posttype, utskriftsfarge, posthåndtering, returmottaker);

                var behandlingsansvarlig = new Behandlingsansvarlig(OrgnummerPosten);
                behandlingsansvarlig.Avsenderidentifikator = "digipost";
                var mpcId = "fysiskpostintegrasjon-" + Guid.NewGuid();

                Forsendelse forsendelse;
                var sdpKlient = ByggDokumentpakkeOgSend(behandlingsansvarlig, postinfo, mpcId, out forsendelse);

                HentKvitteringOgBekreft(sdpKlient, beskrivelse, mpcId, forsendelse);
            }
            catch (Exception e)
            {
                Assert.Fail("Feilet for '{0}', feilmelding: {1}, {2}", beskrivelse, e.Message, e.StackTrace);
            }

        }
        
        private static void HentKvitteringOgBekreft(SikkerDigitalPostKlient sdpKlient, string beskrivelse, string mpcId,
            Forsendelse forsendelse)
        {
            bool hentKvitteringPåNytt = true;

            while (hentKvitteringPåNytt)
            {
                Thread.Sleep(1000);
                var kvitteringsforespørsel = new Kvitteringsforespørsel(Prioritet.Prioritert, mpcId);
                var kvittering = sdpKlient.HentKvittering(kvitteringsforespørsel);

                if (kvittering == null)
                {
                    continue;
                }

                if (kvittering != null)
                {
                    hentKvitteringPåNytt = false;
                }

                Guid konversasjonsId = Guid.Empty;

                if (kvittering is Feilmelding)
                {
                    var feilmelding = (Feilmelding)kvittering;
                    konversasjonsId = feilmelding.KonversasjonsId;
                    Assert.Fail(String.Format("Test '{0}' feilet. Feilmelding fra Meldingsformidler: {1}", beskrivelse,
                        feilmelding.Detaljer));
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
                            "Fikk ikke til å hente kvittering for test '{0}', køen er tom. Var du for rask å hente, " +
                            "eller har noe skjedd galt med hvilken kø du henter fra?", beskrivelse));
                }

                sdpKlient.Bekreft((Forretningskvittering)kvittering);
            }
        }

        private string GetFirstFile(string path)
        {
            var elements = ResourceUtility.GetFiles(path);

            if (elements.Count() == 0)
            {
                throw new FileNotFoundException(String.Format("Kunne ikke finne filen med ressurssti {0}, har du lagt den i 'testdata' og valgt BuildAction = 'Embedded Resource'? " +
                                                              "Husk at stiseparator SKAL være punktum. Dette er ikke en feil.", path));
            }

            return elements.ElementAt(0);
        }

        private static Dokumentpakke ByggDokumentpakke(string hoveddokumentsti, string hoveddoktype, string vedlegg1Sti,
            string vedlegg1Type, string vedlegg2Sti, string vedlegg2Type)
        {
            var hoveddokumentBytes = ResourceUtility.ReadAllBytes(false, hoveddokumentsti);
            var hoveddokument = new Dokument(DateTime.Now.ToString("G") + " - Hoveddokument", hoveddokumentBytes,
                hoveddoktype, "NO", "filnavn");
            var dokumentpakke = new Dokumentpakke(hoveddokument);

            LeggVedleggTilDokumentpakke(dokumentpakke, "Vedlegg1Tittel", vedlegg1Sti, vedlegg1Type, "NO", "Vedlegg1Navn");
            LeggVedleggTilDokumentpakke(dokumentpakke, "Vedlegg2Tittel", vedlegg2Sti, vedlegg2Type, "NO", "Vedlegg2Navn");

            return dokumentpakke;
        }

        private static void LeggVedleggTilDokumentpakke(Dokumentpakke dokumentpakke, string tittel, string sti, string innholdstype, string språkkode, string filnavn)
        {
            if (String.IsNullOrEmpty(sti) || String.IsNullOrEmpty(innholdstype))
                return;

            var bytes = ResourceUtility.ReadAllBytes(false, sti);
            var vedlegg = new Dokument(tittel, bytes, innholdstype, språkkode, filnavn);
            dokumentpakke.LeggTilVedlegg(vedlegg);
        }

        private string GetColumnData(string column)
        {
            return TestContext.DataRow[column].ToString();
        }
    }
}
