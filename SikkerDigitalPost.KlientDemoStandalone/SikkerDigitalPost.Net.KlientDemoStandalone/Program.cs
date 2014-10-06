using System.Security.Cryptography.X509Certificates;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Net.Domene.Entiteter.Post;
using SikkerDigitalPost.Net.KlientApi;

namespace SikkerDigitalPost.Net.KlientDemoStandalone
{
    class Program
    {
        private static void Main(string[] args)
        {
            /*
             * Følgende sertifikater må brukes for å kunne sende digital post
             * 
             * - Mottagersertifikat brukes for å kryptere og signere dokumentpakke som skal til mottagerens postkasse.
             * - TekniskAvsenderSertifikat brukes for sikker kommunikasjon med meldingsformidler.
             */
            X509Certificate2 mottagerSertifikat; //Sertifikat til mottager fra oppslagstjeneste --> kryptering
            X509Certificate2 tekniskAvsenderSertifikat;
                //Dette sertifikatet brukes i kommunikasjon mot meldingsformidler. --> all signering (oppslag)

            /*
             * Nåværende versjon av KlientAPI kan ikke sende meldinger, men kan lage hele meldingen som skal sendes. For å gjøre 
             * dette, setter vi sertifikatene til et tilfeldig fra maskinen
             */

            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var tilfeldigSertifikat = store.Certificates[0];
            store.Close();

            mottagerSertifikat = tilfeldigSertifikat;
            tekniskAvsenderSertifikat = tilfeldigSertifikat;

            /*
             * I dette eksemplet er det Posten som er den som produserer informasjon/brev/post som skal formidles (Behandlingsansvarlig),
             * Posten som er teknisk avsender, og det er Digipostkassen som skal motta meldingen. Derfor er alle organisasjonsnummer
             * identiske. 
             */

            var organisasjonsnummerPosten = "984661185";
            var organisasjonsnummerTekniskAvsender = organisasjonsnummerPosten;
            var organisasjonsnummerMottagerPostkasse = organisasjonsnummerPosten;
            var organisasjonsnummerBehandlingsansvarlig = organisasjonsnummerPosten;

            //Avsender
            var behandlingsansvarlig =
                new Behandlingsansvarlig(new Organisasjonsnummer(organisasjonsnummerBehandlingsansvarlig));
            var tekniskAvsender = new Databehandler(organisasjonsnummerTekniskAvsender, tekniskAvsenderSertifikat);

            //Mottaker
            var mottaker = new Mottaker("04036125433", "ove.jonsen#6K5A", mottagerSertifikat,
                organisasjonsnummerMottagerPostkasse);

            //Digital Post
            var digitalPost = new DigitalPost(mottaker, "Ikke-sensitiv tittel");
           
            //Dokumenter
            string hoveddokument;
            string vedlegg;
            string machineName = System.Environment.MachineName;
            if (machineName.Contains("LEK"))
            {
                hoveddokument = @"C:\sdp\testdata\hoveddokument\hoveddokument.docx";
                vedlegg = @"C:\sdp\testdata\vedlegg\VedleggsGris.docx";
            }
            else
            {
                hoveddokument = @"C:\Prosjekt\DigiPost\Temp\TestData\hoveddokument\hoveddokument.docx";
                vedlegg = @"C:\Prosjekt\DigiPost\Temp\TestData\vedlegg\VedleggsGris.docx";
            }
            
            //Forsendelse
            var dokumentpakke = new Dokumentpakke(new Dokument("Hoveddokument", hoveddokument, "text/docx"));
            dokumentpakke.LeggTilVedlegg(new Dokument("Vedleggsgris", vedlegg, "text/docx", "EN"));
            var forsendelse = new Forsendelse(behandlingsansvarlig, digitalPost, dokumentpakke);

            //Send
            var sikkerDigitalPostKlient = new SikkerDigitalPostKlient(tekniskAvsender);

            sikkerDigitalPostKlient.Send(forsendelse);
        }
    }
}
