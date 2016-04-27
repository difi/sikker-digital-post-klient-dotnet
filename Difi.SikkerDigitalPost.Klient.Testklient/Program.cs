using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using ApiClientShared;
using Common.Logging;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Testklient.Properties;
using Difi.SikkerDigitalPost.Klient.XmlValidering;

namespace Difi.SikkerDigitalPost.Klient.Testklient
{
    internal class Program
    {
        private const string MpcId = "queue1";
        private const bool ErDigitalPostMottaker = true;
        private const bool ErNorskBrev = true;

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            SendPost();
        }

        private static void SendPost()
        {
            /*
             * I dette eksemplet er det Posten som er den som produserer informasjon/brev/post som skal formidles (Avsender),
             * Posten som er databehandler, og det er Digipostkassen som skal motta meldingen. 
             */

            Log.Debug(@"--- STARTER Å SENDE POST ---");

            /*
             * SETT OPP MOTTAKER OG INNSTILLINGER
             */
            var postInfo = GenererPostInfo(ErDigitalPostMottaker, ErNorskBrev);
            var avsender = new Avsender(Settings.Default.OrgnummerPosten);

            var databehandler = new Databehandler(Settings.Default.OrgnummerPosten,
                Settings.Default.DatabehandlerSertifikatThumbprint);
            avsender.Avsenderidentifikator = "digipost";

            var forsendelse = GenererForsendelse(avsender, postInfo);
            var klientkonfigurasjon = SettOppKlientkonfigurasjon();
            klientkonfigurasjon.AktiverLagringAvDokumentpakkeTilDisk(@"C:\Users\aas\Downloads\");

            var sikkerDigitalPostKlient = new SikkerDigitalPostKlient(databehandler, klientkonfigurasjon);

            /**
             * SEND POST OG MOTTA KVITTERINGER
             */
            SendPost(sikkerDigitalPostKlient, forsendelse);

            Log.Debug(@"--- STARTER Å HENTE KVITTERINGER ---");

            HentKvitteringer(sikkerDigitalPostKlient);

            Console.WriteLine();
            Log.Debug(@"--- FERDIG Å SENDE POST OG MOTTA KVITTERINGER :) --- ");
            Console.ReadKey();
        }

        private static async void SendPost(SikkerDigitalPostKlient sikkerDigitalPostKlient, Forsendelse forsendelse)
        {
            var transportkvittering = await sikkerDigitalPostKlient.SendAsync(forsendelse);
            Log.Debug(@" > Post sendt. Status er ...");

            if (transportkvittering.GetType() == typeof (TransportOkKvittering))
            {
                WriteToConsoleWithColor(" > OK! En transportkvittering ble hentet og alt gikk fint.");
            }

            if (transportkvittering.GetType() == typeof (TransportFeiletKvittering))
            {
                var feiletkvittering = (TransportFeiletKvittering) transportkvittering;
                WriteToConsoleWithColor(
                    $" > {feiletkvittering.Alvorlighetsgrad}. Nå gikk det galt her. {feiletkvittering.Beskrivelse}",
                    true);
            }
        }

        private static async void HentKvitteringer(SikkerDigitalPostKlient sikkerDigitalPostKlient)
        {
            Log.Debug("");

            Log.Debug(@" > Starter å hente kvitteringer ...");

            Thread.Sleep(3000);

            while (true)
            {
                var kvitteringsForespørsel = new Kvitteringsforespørsel(Prioritet.Prioritert, MpcId);
                Log.Debug($" > Henter kvittering på kø '{kvitteringsForespørsel.Mpc}'...");

                var kvittering = await sikkerDigitalPostKlient.HentKvitteringAsync(kvitteringsForespørsel);

                if (kvittering is TomKøKvittering)
                {
                    Console.WriteLine($"  - Kø '{kvitteringsForespørsel.Mpc}' er tom. Stopper å hente meldinger. ");
                    break;
                }

                if (kvittering is TransportFeiletKvittering)
                {
                    var feil = ((TransportFeiletKvittering) kvittering).Beskrivelse;
                    WriteToConsoleWithColor(
                        "En feil skjedde under transport. Forespørsel for kvittering ble ikke godtatt av Meldingsformidler: " +
                        feil, true);
                    break;
                }

                if (kvittering is Leveringskvittering)
                {
                    WriteToConsoleWithColor("  - En leveringskvittering ble hentet!");
                }

                if (kvittering is Åpningskvittering)
                {
                    WriteToConsoleWithColor("  - Har du sett. Noen har åpnet et brev. Moro.");
                }

                if (kvittering is Returpostkvittering)
                    WriteToConsoleWithColor("  - Du har fått en returpostkvittering for fysisk post.");

                if (kvittering is Mottakskvittering)
                    WriteToConsoleWithColor("  - Kvittering på sending av fysisk post mottatt.");

                if (kvittering is Feilmelding)
                {
                    var feil = (Feilmelding) kvittering;
                    WriteToConsoleWithColor("  - En feilmelding ble hentet :" + feil.Detaljer, true);
                }

                Console.WriteLine(@"  - Bekreftelse på mottatt kvittering sendes ...");
                sikkerDigitalPostKlient.Bekreft((Forretningskvittering) kvittering);
                Console.WriteLine(@"   - Kvittering sendt.");
            }
        }

        private static Klientkonfigurasjon SettOppKlientkonfigurasjon()
        {
            var klientkonfigurasjon = new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø);
            //klientkonfigurasjon.LoggXmlTilFil = false;
            //klientkonfigurasjon.StandardLoggSti = @"Z:\aleksander sjafjell On My Mac\Development\Shared\sdp-data\Logg";
            return klientkonfigurasjon;
        }

        private static Forsendelse GenererForsendelse(Avsender avsender, PostInfo postInfo)
        {
            var resourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Testklient.Resources");

            var hoveddokument = resourceUtility.ReadAllBytes(true, "Hoveddokument.pdf");
            var vedlegg = resourceUtility.ReadAllBytes(true, "Vedlegg.txt");

            //Forsendelse
            var dokumentpakke =
                new Dokumentpakke(new Dokument("Sendt" + DateTime.Now, hoveddokument, "application/pdf", "NO",
                    "OWASP TOP 10.pdf"));
            dokumentpakke.LeggTilVedlegg(new Dokument("Vedlegg", vedlegg, "text/plain", "NO", "Vedlegg.txt"));
            var forsendelse = new Forsendelse(avsender, postInfo, dokumentpakke, Prioritet.Prioritert, MpcId);

            return forsendelse;
        }

        private static void WriteToConsoleWithColor(string message, bool isError = false)
        {
            Console.ForegroundColor = isError ? ConsoleColor.Red : ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static PostInfo GenererPostInfo(bool erDigitalPostMottaker, bool erNorskBrev)
        {
            var resourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Testklient.Resources.Sertifikater");

            PostInfo postInfo;
            PostMottaker mottaker;
            var sertifikat =
                new X509Certificate2(resourceUtility.ReadAllBytes(true, "testmottakerFraOppslagstjenesten.pem"));

            if (erDigitalPostMottaker)
            {
                mottaker = new DigitalPostMottaker(Settings.Default.MottakerPersonnummer,
                    Settings.Default.MottakerDigipostadresse, sertifikat, Settings.Default.OrgnummerPosten
                    );

                postInfo = new DigitalPostInfo((DigitalPostMottaker) mottaker, "Ikke-sensitiv tittel",
                    Sikkerhetsnivå.Nivå3, true);
                ((DigitalPostInfo) postInfo).Virkningstidspunkt = DateTime.Now.AddMinutes(0);

                ((DigitalPostInfo) postInfo).SmsVarsel = new SmsVarsel("12345678", "Et lite varsel pr SMS.");
            }
            else
            {
                Adresse adresse;
                if (erNorskBrev)
                    adresse = new NorskAdresse("0566", "Oslo");
                else
                    adresse = new UtenlandskAdresse("SE", "Saltkråkan 22");

                mottaker = new FysiskPostMottaker("Rolf Rolfsen", adresse,
                    sertifikat, Settings.Default.OrgnummerPosten);

                var returMottaker = new FysiskPostReturmottaker("ReturKongen", new NorskAdresse("1533", "Søppeldynga"));

                postInfo = new FysiskPostInfo((FysiskPostMottaker) mottaker, Posttype.A, Utskriftsfarge.SortHvitt,
                    Posthåndtering.DirekteRetur, returMottaker);
            }
            return postInfo;
        }
    }
}