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

        private class PostkasseSettings
        {
            /*
            * Følgende sertifikater må brukes for å kunne sende digital post
            * 
            * - Mottagersertifikat brukes for å kryptere og signere dokumentpakke som skal til mottagerens postkasse.
            * - TekniskAvsenderSertifikat brukes for sikker kommunikasjon med meldingsformidler.
            */
            public X509Certificate2 Avsendersertifikat { get; set; }
            public X509Certificate2 Mottakersertifikat { get; set; }

            public string OrgNummerBehandlingsansvarlig { get; set; }
            public string OrgNummerDatabehandler { get; set; }
            public string OrgnummerPostkasse { get; set; }
            public string Personnummer { get; set; }
            public string Postkasseadresse { get; set; }

            private PostkasseSettings(X509Certificate2 avsendersertifikat, X509Certificate2 mottakersertifikat, string orgNummerBehandlingsansvarlig, string orgNummerDatabehandler, string orgnummerPostkasse,
                string personnummer, string postkasseadresse)
            {
                Avsendersertifikat = avsendersertifikat;
                Mottakersertifikat = mottakersertifikat;
                OrgNummerBehandlingsansvarlig = orgNummerBehandlingsansvarlig;
                OrgNummerDatabehandler = orgNummerDatabehandler;
                OrgnummerPostkasse = orgnummerPostkasse;
                Personnummer = personnummer;
                Postkasseadresse = postkasseadresse;
            }

            public static PostkasseSettings GetEboks()
            {
                X509Certificate2 tekniskAvsenderSertifikat;
                {
                    X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadOnly);
                    tekniskAvsenderSertifikat = store.Certificates.Find(
                        X509FindType.FindByThumbprint, "8702F5E55217EC88CF2CCBADAC290BB4312594AC", true)[0];
                    store.Close();
                }

                X509Certificate2 mottakerSertifikat;
                {
                    X509Store store2 = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
                    store2.Open(OpenFlags.ReadOnly);
                    mottakerSertifikat =
                        store2.Certificates.Find(X509FindType.FindByThumbprint, "B43CAAA0FBEE6C8DA85B47D1E5B7BCAB42AB9ADD", true)[0];
                    store2.Close();
                }

                var orgnummerPosten = "984661185";
                var orgnummerDatabehandler = orgnummerPosten;
                var orgnummerBehandlingsansvarlig = orgnummerPosten;
                var orgnummerPostkasse = "996460320";
                var mottakerPersonnummer = "01043100358";
                var mottakerPostkasse = "0000485509";

                return new PostkasseSettings(tekniskAvsenderSertifikat,mottakerSertifikat, orgnummerBehandlingsansvarlig, orgnummerDatabehandler, orgnummerPostkasse, mottakerPersonnummer, mottakerPostkasse);   
            }

            public static PostkasseSettings GetPosten ()
            {
                X509Certificate2 tekniskAvsenderSertifikat;
                {
                    X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadOnly);
                    tekniskAvsenderSertifikat = store.Certificates.Find(
                        X509FindType.FindByThumbprint, "8702F5E55217EC88CF2CCBADAC290BB4312594AC", true)[0];
                    store.Close();
                }

                X509Certificate2 mottakerSertifikat;
                {
                    X509Store store2 = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
                    store2.Open(OpenFlags.ReadOnly);
                    mottakerSertifikat =
                        store2.Certificates.Find(X509FindType.FindByThumbprint, "B43CAAA0FBEE6C8DA85B47D1E5B7BCAB42AB9ADD", true)[0];
                    store2.Close();
                }

                var orgnummerPosten = "984661185";
                var orgnummerDatabehandler = orgnummerPosten;
                var orgnummerBehandlingsansvarlig = orgnummerPosten;
                var orgnummerPostkasse = orgnummerPosten;
                var mottakerPersonnummer = "04036125433";
                var mottakerPostkasse = "ove.jonsen#6K5A";

                return new PostkasseSettings(tekniskAvsenderSertifikat, mottakerSertifikat, orgnummerBehandlingsansvarlig, orgnummerDatabehandler, orgnummerPostkasse, mottakerPersonnummer, mottakerPostkasse);   
            }
        }

        static void Main(string[] args)
        {
           
            
            /*
             * I dette eksemplet er det Posten som er den som produserer informasjon/brev/post som skal formidles (Behandlingsansvarlig),
             * Posten som er teknisk avsender, og det er Digipostkassen som skal motta meldingen. Derfor er alle organisasjonsnummer
             * identiske. 
             */

            PostkasseSettings postkasseSettings = PostkasseSettings.GetPosten();

            //Avsender
            var behandlingsansvarlig = new Behandlingsansvarlig(new Organisasjonsnummer(postkasseSettings.OrgNummerBehandlingsansvarlig));
            var tekniskAvsender = new Databehandler(postkasseSettings.OrgNummerDatabehandler, postkasseSettings.Avsendersertifikat);

            //Mottaker
            var mottaker = new Mottaker(postkasseSettings.Personnummer, postkasseSettings.Postkasseadresse, postkasseSettings.Mottakersertifikat, postkasseSettings.OrgnummerPostkasse);

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
            int count = 0;
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

                count++;

                //if (kvittering.GetType() == typeof(Feilmelding))
                //{
                //    throw new Exception("Du fikk en feiletkvittering, men det er ikke sikkert du genererte den nå nettopp.");
                //}
                
                //Bekreft mottak av kvittering.
                if(kjør)
                    sikkerDigitalPostKlient.Bekreft(kvittering);
            }

            int i = 0;

        }
    }
}






