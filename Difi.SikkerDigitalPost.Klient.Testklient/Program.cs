using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
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
using System.Linq;
using Digipost.Api.Client.Shared.Resources.Resource;
using log4net;

namespace Difi.SikkerDigitalPost.Klient.Testklient
{
    internal class Program
    {
        private static string MpcId = RandomMpcId(); // Avoids channel conflicts
        private const bool ErDigitalPostMottaker = true;
        private const bool ErNorskBrev = true;

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            var erDigipost = false;
            var digipostUserInfo = GenererPostInfo(ErDigitalPostMottaker, ErNorskBrev, erDigipost);
            SendPost(digipostUserInfo);
        }

        private static void SendPost(PostInfo postInfo)
        {
            /*
             * I dette eksemplet er det Posten som er den som produserer informasjon/brev/post som skal formidles (Avsender),
             * Posten som er databehandler, og det er Digipostkassen som skal motta meldingen. 
             */

            Log.Debug(@"--- STARTER Å SENDE POST ---");

            /*
             * SETT OPP MOTTAKER OG INNSTILLINGER
             */
            
            var avsender = new Avsender(new Organisasjonsnummer(Settings.Default.DifiOrgNummer));

            var databehandler = new Databehandler(
                new Organisasjonsnummer(Settings.Default.DifiOrgNummer),
                Settings.Default.DifiSertifikatThumbprint);


            var forsendelse = GenererForsendelse(avsender, postInfo);
            var klientkonfigurasjon = SettOppKlientkonfigurasjon();
            klientkonfigurasjon.AktiverLagringAvDokumentpakkeTilDisk(@"C:\Users\User\Downloads\");

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
            var transportkvittering = await sikkerDigitalPostKlient.SendAsync(forsendelse).ConfigureAwait(false);
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

                var kvittering = await sikkerDigitalPostKlient.HentKvitteringAsync(kvitteringsForespørsel).ConfigureAwait(false);

                if (kvittering is TomKøKvittering)
                {
                    Console.WriteLine($"  - Kø '{kvitteringsForespørsel.Mpc}' er tom. Venter og prover igjen. ");
                    Thread.Sleep(3000);
                    continue;
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

        private static string RandomMpcId()
        {
            Random random = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyz";
            var length = 6;
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
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

            var hoveddokument = resourceUtility.ReadAllBytes("Hoveddokument.pdf");
            var vedlegg = resourceUtility.ReadAllBytes("Vedlegg.txt");

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

        private static PostInfo GenererPostInfo(bool erDigitalPostMottaker, bool erNorskBrev, bool erDigipost)
        {
            var resourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Testklient.Resources.Sertifikater");

            PostInfo postInfo;
            PostMottaker mottaker;
            X509Certificate2 sertifikat;
            if (erDigipost)
            {
                sertifikat =
                new X509Certificate2(resourceUtility.ReadAllBytes("testmottakerFraOppslagstjenesten_digipost.pem"));
                mottaker = new DigitalPostMottaker(Settings.Default.DigipostMottakerPersonnummer,
                    Settings.Default.DigipostMottakerDigipostadresse, sertifikat, new Organisasjonsnummer(Settings.Default.PostenOrgNr));
            }
            else
            {
                sertifikat =
                new X509Certificate2(resourceUtility.ReadAllBytes("testmottakerFraOppslagstjenesten_eboks.pem"));
                mottaker = new DigitalPostMottaker(Settings.Default.EboksMottakerPersonnummer,
                    Settings.Default.EboksMottakerEboksadresse, sertifikat, new Organisasjonsnummer(Settings.Default.EboksOrgNr));
            }

            if (erDigitalPostMottaker)
            {
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
                    sertifikat, new Organisasjonsnummer(Settings.Default.PostenOrgNr));

                var returMottaker = new FysiskPostReturmottaker("ReturKongen", new NorskAdresse("1533", "Søppeldynga"));

                postInfo = new FysiskPostInfo((FysiskPostMottaker) mottaker, Posttype.A, Utskriftsfarge.SortHvitt,
                    Posthåndtering.DirekteRetur, returMottaker);
            }
            return postInfo;
        }
        
    }
}