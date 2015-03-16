using System;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
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

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", @"|DataDirectory|\testdata\integrasjon\digitalpost.csv", "digitalpost#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SendDigitalPost()
        {
            var beskrivelse = "Ingen beskrivelse";
            try
            {
                //Arrange
                beskrivelse = GetColumnData("beskrivelse");
                var personnummer = "04036125433"; //GetColumnData("personnummer");
                var postkasseadresse = GetColumnData("postkasseadresse");
                var hoveddoktype = GetColumnData("hoveddokumenttype");
                var hoveddokumentsti = GetFirstFile(GetColumnData("hoveddokument"));
                var vedlegg1Type = GetColumnData("vedlegg1type");
                var vedlegg1Sti = GetFirstFile(GetColumnData("vedlegg1"));
                var vedlegg2Type = GetColumnData("vedlegg2type");
                var vedlegg2Sti = GetFirstFile(GetColumnData("vedlegg2"));

                var mottaker = new DigitalPostMottaker(personnummer, postkasseadresse,
                    MottakerSertifikat("B43CAAA0FBEE6C8DA85B47D1E5B7BCAB42AB9ADD"), OrgnummerPosten);
                var postInfo = new DigitalPostInfo(mottaker, "Ikkesensitiv tittel fra Endetester", Sikkerhetsnivå.Nivå3,
                    false);
                var behandlingsansvarlig = new Behandlingsansvarlig(OrgnummerPosten);
                behandlingsansvarlig.Avsenderidentifikator = "digipost";

                var databehandler = new Databehandler(OrgnummerPosten,
                    AvsenderSertifkat("8702F5E55217EC88CF2CCBADAC290BB4312594AC"));
                var dokumentpakke = ByggDokumentpakke(hoveddokumentsti, hoveddoktype, vedlegg1Sti, vedlegg1Type,
                    vedlegg2Sti, vedlegg2Type);

                //Act
                var sdpKlient = new SikkerDigitalPostKlient(databehandler, new Klientkonfigurasjon
                {
                    MeldingsformidlerUrl = new Uri("https://qaoffentlig.meldingsformidler.digipost.no/api/ebms")
                });

                var mpcId = "testerqueue-"+Guid.NewGuid();
                var forsendelse = new Forsendelse(behandlingsansvarlig, postInfo, dokumentpakke, Prioritet.Prioritert,
                    mpcId);
                var transportkvittering = sdpKlient.Send(forsendelse);

                //Assert
                if (transportkvittering.GetType() == typeof (TransportFeiletKvittering))
                {
                    var feil = ((TransportFeiletKvittering) transportkvittering).Beskrivelse;
                    Assert.Fail(feil);
                }

                HentKvitteringOgBekreft(sdpKlient, beskrivelse, mpcId, forsendelse);
            }
            catch (Exception e)
            {
                Assert.Fail(String.Format("Feilet for '{0}', feilmelding: {1}, {2}", beskrivelse, e.Message, e.StackTrace));
            }

        }

        private static void HentKvitteringOgBekreft(SikkerDigitalPostKlient sdpKlient, string beskrivelse, string mpcId,
            Forsendelse forsendelse)
        {
            bool hentKvitteringPåNytt = true;

            while (hentKvitteringPåNytt)
            {
                Thread.Sleep(500);
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
                    var feilmelding = (Feilmelding) kvittering;
                    konversasjonsId = feilmelding.KonversasjonsId;
                    Assert.Fail(String.Format("Test '{0}' feilet. Feilmelding fra Meldingsformidler: {1}", beskrivelse,
                        feilmelding.Detaljer));
                }

                if (kvittering is Leveringskvittering)
                {
                    var leveringskvittering = (Leveringskvittering) kvittering;
                    konversasjonsId = leveringskvittering.KonversasjonsId;
                }

                if (konversasjonsId.ToString() != forsendelse.KonversasjonsId.ToString())
                {
                    throw new FieldAccessException(
                        String.Format(
                            "Fikk ikke til å hente kvittering for test '{0}', køen er tom. Var du for rask å hente, " +
                            "eller har noe skjedd galt med hvilken kø du henter fra?", beskrivelse));
                }

                sdpKlient.Bekreft((Forretningskvittering) kvittering);
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
            string vedlegg1Type, string vedlegg2Sti,string vedlegg2Type)
        {
            var hoveddokumentBytes = ResourceUtility.ReadAllBytes(false, hoveddokumentsti);
            var hoveddokument = new Dokument(DateTime.Now.ToString("G") + " - Hoveddokument", hoveddokumentBytes,
                hoveddoktype, "NO", "filnavn");
            var dokumentpakke = new Dokumentpakke(hoveddokument);

            LeggVedleggTilDokumentpakke(dokumentpakke, "Vedlegg1Tittel", vedlegg1Sti, vedlegg1Type, "NO", "Vedlegg1Navn");
            LeggVedleggTilDokumentpakke(dokumentpakke, "Vedlegg2Tittel", vedlegg2Sti, vedlegg2Type, "NO", "Vedlegg2Navn");

            return dokumentpakke;
        }

        private static void LeggVedleggTilDokumentpakke(Dokumentpakke dokumentpakke, string tittel, string sti, string innholdstype, string språkkode,
            string filnavn)
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

        private static X509Certificate2 AvsenderSertifkat(string hash)
        {
            X509Store storeMy = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            X509Certificate2 tekniskAvsenderSertifikat;
            try
            {
                storeMy.Open(OpenFlags.ReadOnly);
                tekniskAvsenderSertifikat = storeMy.Certificates.Find(
                    X509FindType.FindByThumbprint, hash, true)[0];
            }
            catch (Exception e)
            {
                throw new InstanceNotFoundException("Kunne ikke finne avsendersertifikat for testing. Har du lagt det til slik guiden tilsier? (https://github.com/difi/sikker-digital-post-net-klient#legg-inn-avsendersertifikat-i-certificate-store) ", e);
            }
            storeMy.Close();
            return tekniskAvsenderSertifikat;
        }

        private static X509Certificate2 MottakerSertifikat(string hash)
        {
            var storeTrusted = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
            X509Certificate2 mottakerSertifikat;
            try
            {
                storeTrusted.Open(OpenFlags.ReadOnly);
                mottakerSertifikat =
                    storeTrusted.Certificates.Find(X509FindType.FindByThumbprint, hash, true)[0];
            }
            catch (Exception e)
            {
                throw new InstanceNotFoundException("Kunne ikke finne mottakersertifikat for testing. Har du lagt det til slik guiden tilsier? (https://github.com/difi/sikker-digital-post-net-klient#legg-inn-mottakersertifikat-i-certificate-store) ", e);
            }
            storeTrusted.Close();
            return mottakerSertifikat;
        }

    }
}
