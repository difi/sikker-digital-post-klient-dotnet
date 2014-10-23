using System;
using System.Linq;
using SikkerDigitalPost.Domene.Entiteter;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Domene.Enums;
using SikkerDigitalPost.Klient;
using SikkerDigitalPost.Klient.Utilities;

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

            PostkasseInnstillinger postkasseInnstillinger = PostkasseInnstillinger.GetEboks();

            //Avsender
            var behandlingsansvarlig = new Behandlingsansvarlig(new Organisasjonsnummer(postkasseInnstillinger.OrgNummerBehandlingsansvarlig));
            var tekniskAvsender = new Databehandler(postkasseInnstillinger.OrgNummerDatabehandler, postkasseInnstillinger.Avsendersertifikat);


            var mottaker = HentMottaker(postkasseInnstillinger, false);

            //Digital Post
            var digitalPost = new DigitalPost(mottaker, "Ikke-sensitiv tittel", Sikkerhetsnivå.Nivå4, åpningskvittering: false);

            string hoveddokument = FileUtility.AbsolutePath("testdata", "hoveddokument", "hoveddokument.txt");
            string vedlegg = FileUtility.AbsolutePath("testdata", "vedlegg", "Vedlegg.txt");
            
            //Forsendelse
            string mpcId = "hest";
            var dokumentpakke = new Dokumentpakke(new Dokument("Hoveddokument", hoveddokument, "text/plain"));
            dokumentpakke.LeggTilVedlegg(new Dokument("Vedlegg", vedlegg, "text/plain", "EN"));
            var forsendelse = new Forsendelse(behandlingsansvarlig, digitalPost, dokumentpakke, Prioritet.Prioritert,mpcId,"NO");

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

            Console.WriteLine(" > Starter å hente kvitteringer ...");
            
            while (true)
            {
                var kvitteringsForespørsel = new Kvitteringsforespørsel(Prioritet.Prioritert, mpcId);
                Console.WriteLine(" > Henter kvittering på kø '{0}'...", kvitteringsForespørsel.Mpc);

                Forretningskvittering kvittering = sikkerDigitalPostKlient.HentKvittering(kvitteringsForespørsel);

                if (kvittering == null)
                {
                    Console.WriteLine( "  - Kø '{0}' er tom. Stopper å hente meldinger. ", kvitteringsForespørsel.Mpc);
                    break;
                }

                if (kvittering.GetType() == typeof (Leveringskvittering))
                {
                    Console.WriteLine("  - En leveringskvittering ble hentet!");
                }

                if (kvittering.GetType() == typeof (Åpningskvittering))
                {
                    Console.WriteLine("  - Har du sett. Noen har åpnet et brev. Moro.");
                }

                if (kvittering.GetType() == typeof(Feilmelding))
                {
                    Console.WriteLine("  - En feilmelding ble hentet, men den kan være gammel ...");
                }
                
                Console.WriteLine("  - Bekreftelse på mottatt kvittering sendes ...");
                sikkerDigitalPostKlient.Bekreft(kvittering);
                Console.WriteLine("   - Kvittering sendt.");
            }


            Console.WriteLine();
            Console.WriteLine("--- FERDIG Å SENDE POST OG MOTTA KVITTERINGER :) --- ");
            Console.ReadKey();
        }

        private static Mottaker HentMottaker(PostkasseInnstillinger postkasseInnstillinger, bool hentFraKontaktregisteret = true)
        {
            Mottaker mottaker;
            if (hentFraKontaktregisteret)
            {
                mottaker = Kontaktregisteret.HentPersoner(new[] {postkasseInnstillinger.Personnummer}).ElementAt(0);
            }
            else
            {
                mottaker = new Mottaker(postkasseInnstillinger.Personnummer, postkasseInnstillinger.Postkasseadresse,
                    postkasseInnstillinger.Mottakersertifikat, postkasseInnstillinger.OrgnummerPostkasse);
            }
            return mottaker;
        }
    }
}






