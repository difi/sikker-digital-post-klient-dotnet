using System;
using System.Security.Cryptography.X509Certificates;
using KontaktregisteretGateway;
using KontaktregisteretGateway.Difi;
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

            //var service = new X509Certificate2(@"../../../Kontaktregisteretsertifikater/idporten-ver2.difi.no-v2.crt", "changeit");
            //var client = new X509Certificate2(@"../../../Kontaktregisteretsertifikater/WcfClient.pfx", "changeit");
            //var settings = new DifiGatewaySettings(client, service);

            //var _kontaktregisteretGateway = new KontaktregisteretGateway.KontaktregisteretGateway(settings);

            ////Hent person fra difi! 
            //var request = new HentPersonerForespoersel();
            //request.informasjonsbehov = new informasjonsbehov[1];
            //request.informasjonsbehov[0] = informasjonsbehov.Kontaktinfo;
            //request.personidentifikator = new string[1];
            //request.personidentifikator[0] = postkasseInnstillinger.Personnummer;

            //var personer = _kontaktregisteretGateway.HentPersoner(request);

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
                    break;
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






