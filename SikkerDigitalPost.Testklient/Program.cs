using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using KontaktregisteretGateway;
using KontaktregisteretGateway.Difi;
using SikkerDigitalPost.Domene.Entiteter;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Domene.Enums;
using SikkerDigitalPost.Klient;
using SikkerDigitalPost.Klient.Utilities;
using Person = SikkerDigitalPost.Domene.Entiteter.Aktører.Person;

namespace SikkerDigitalPost.Testklient
{
    class Program
    {
        static void Main(string[] args)
        {
           /*
             * I dette eksemplet er det Posten som er den som produserer informasjon/brev/post som skal formidles (Behandlingsansvarlig),
             * Posten som er teknisk avsender, og det er Digipostkassen som skal motta meldingen. Derfor er alle organisasjonsnummer
             * identiske. 
             */

            PostkasseInnstillinger postkasseInnstillinger = PostkasseInnstillinger.GetPosten();

            //Avsender
            var behandlingsansvarlig = new Behandlingsansvarlig(new Organisasjonsnummer(postkasseInnstillinger.OrgNummerBehandlingsansvarlig));
            var tekniskAvsender = new Databehandler(postkasseInnstillinger.OrgNummerDatabehandler, postkasseInnstillinger.Avsendersertifikat);


            Mottaker mottaker;
            bool hentFraKontaktregisteret = true;
            if (hentFraKontaktregisteret)
            {
                mottaker = Kontaktregisteret.HentPersoner(new[] {postkasseInnstillinger.Personnummer}).ElementAt(0);
            }
            else
            {
                mottaker = new Mottaker(postkasseInnstillinger.Personnummer, postkasseInnstillinger.Postkasseadresse, postkasseInnstillinger.Mottakersertifikat, postkasseInnstillinger.OrgnummerPostkasse);
            }

            //Digital Post
            var digitalPost = new DigitalPost(mottaker, "Ikke-sensitiv tittel", Sikkerhetsnivå.Nivå4, åpningskvittering: false);

            string hoveddokument = FileUtility.AbsolutePath("testdata", "hoveddokument", "hoveddokument.txt");
            string vedlegg = FileUtility.AbsolutePath("testdata", "vedlegg", "Vedlegg.txt");
            
            //Forsendelse
            var dokumentpakke = new Dokumentpakke(new Dokument("Hoveddokument", hoveddokument, "text/plain"));
            dokumentpakke.LeggTilVedlegg(new Dokument("Vedlegg", vedlegg, "text/plain", "EN"));
            var forsendelse = new Forsendelse(behandlingsansvarlig, digitalPost, dokumentpakke, Prioritet.Prioritert,"NO");

            //Send
            var klientkonfigurasjon = new Klientkonfigurasjon();
            klientkonfigurasjon.MeldingsformidlerUrl = new Uri("https://qaoffentlig.meldingsformidler.digipost.no/api/ebms");
            var sikkerDigitalPostKlient = new SikkerDigitalPostKlient(tekniskAvsender,klientkonfigurasjon);

            Console.WriteLine("--- STARTER Å SENDE POST ---");

            Transportkvittering transportkvittering = sikkerDigitalPostKlient.Send(forsendelse);
            Console.WriteLine(" > Post sendt. Status er ...");

            if (transportkvittering.GetType() == typeof (TransportOkKvittering))
            {
                Console.WriteLine(" > OK! En transportkvittering ble hentet og alt gikk fint.");
            }

            if (transportkvittering.GetType() == typeof (TransportFeiletKvittering))
            {
                var feiletkvittering = (TransportFeiletKvittering) transportkvittering;
                Console.WriteLine(" > {0}. Nå gikk det galt her. {1}", feiletkvittering.Alvorlighetsgrad, feiletkvittering.Beskrivelse);
            }

            Console.WriteLine();
            Console.WriteLine("--- STARTER Å HENTE KVITTERINGER ---");

            var kjør = true;
            while (kjør)
            {
                Console.WriteLine(" > Henter kvittering ...");

                //Hent kvitterings
                var kvitteringsForespørsel = new Kvitteringsforespørsel(Prioritet.Prioritert);
                Forretningskvittering kvittering = sikkerDigitalPostKlient.HentKvittering(kvitteringsForespørsel);

                if (kvittering == null)
                {
                    Console.WriteLine( "  - Tom kvitteringskø. Stopper å hente meldinger. ");
                    kjør = false;
                    break;
                }

                if (kvittering.GetType() == typeof (Leveringskvittering))
                {
                    Console.WriteLine("  - En leveringskvittering ble hentet!");
                }

                if (kvittering.GetType() == typeof(Feilmelding))
                {
                    Console.WriteLine("  - En feilmelding ble hentet!");
                }


                if (kvittering.GetType() == typeof(Feilmelding))
                {
                  Console.WriteLine("  - Du fikk en feiletkvittering, men det er ikke sikkert du genererte den nå nettopp.");
                }
                
                //Bekreft mottak av kvittering.
                if (kjør)
                {
                    Console.WriteLine("  - Bekreftelse på mottatt kvittering sendes ...");
                    sikkerDigitalPostKlient.Bekreft(kvittering);
                    Console.WriteLine("   - Kvittering sendt.");
                }
            }

            Console.WriteLine();
            Console.WriteLine("--- FERDIG Å SENDE MELDING OG MOTTA KVITTERINGER :) --- ");
            Console.ReadKey();
        }
    }
}






