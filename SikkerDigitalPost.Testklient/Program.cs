using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
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

            PostkasseInnstillinger postkasseInnstillinger = PostkasseInnstillinger.GetPosten();

            //Avsender
            var behandlingsansvarlig = new Behandlingsansvarlig(new Organisasjonsnummer(postkasseInnstillinger.OrgNummerBehandlingsansvarlig));
            var tekniskAvsender = new Databehandler(postkasseInnstillinger.OrgNummerDatabehandler, postkasseInnstillinger.Avsendersertifikat);

            //Mottaker
            var mottaker = new Mottaker(postkasseInnstillinger.Personnummer, postkasseInnstillinger.Postkasseadresse, postkasseInnstillinger.Mottakersertifikat, postkasseInnstillinger.OrgnummerPostkasse);

            //Digital Post
            var digitalPost = new DigitalPost(mottaker, "Ikke-sensitiv tittel");

            string hoveddokument = FileUtility.AbsolutePath("testdata", "hoveddokument", "hoveddokument.txt");
            string vedlegg = FileUtility.AbsolutePath("testdata", "vedlegg", "Vedlegg.txt");
            
            //Forsendelse
            var dokumentpakke = new Dokumentpakke(new Dokument("Hoveddokument", hoveddokument, "text/plain"));
            dokumentpakke.LeggTilVedlegg(new Dokument("Vedlegg", vedlegg, "text/plain", "EN"));
            var forsendelse = new Forsendelse(behandlingsansvarlig, digitalPost, dokumentpakke, Prioritet.Prioritert,"NO");

            //Send
            var sikkerDigitalPostKlient = new SikkerDigitalPostKlient(tekniskAvsender);

            Transportkvittering transportkvittering = sikkerDigitalPostKlient.Send(forsendelse);

            var kjør = true;
            while (kjør)
            {
                //Hent kvittering
                var kvitteringsForespørsel = new Kvitteringsforespørsel(Prioritet.Prioritert);
                Forretningskvittering kvittering = sikkerDigitalPostKlient.HentKvittering(kvitteringsForespørsel);
                
                if (kvittering == null)
                {
                    kjør = false;
                   // throw new Exception("Denne meldingskøen er tom.");
                }
                
                if (kvittering.GetType() == typeof(Feilmelding))
                {
                  // throw new Exception("Du fikk en feiletkvittering, men det er ikke sikkert du genererte den nå nettopp.");
                }
                
                //Bekreft mottak av kvittering.
                if(kjør)
                    sikkerDigitalPostKlient.Bekreft(kvittering);
            }
        }
    }
}






