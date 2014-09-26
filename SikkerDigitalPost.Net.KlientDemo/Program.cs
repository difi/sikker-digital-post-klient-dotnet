using System.IO;
using System.Security.Cryptography.X509Certificates;
using SikkerDigitalPost.Net.Domene;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Manifest;
using SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Signatur;
using SikkerDigitalPost.Net.KlientApi;

namespace SikkerDigitalPost.Net.KlientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var nøkkelpar = new Sertifikatbutikk();

            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certificate = store.Certificates[0];
            store.Close();
            
            //Avsender
            var orgNrAvsender = new Organisasjonsnummer("984661185");
            var behandlingsansvarlig = new Behandlingsansvarlig(orgNrAvsender);
            
            //Mottaker
            var orgNrMottaker = new Organisasjonsnummer("984661185");
            var mottaker = new Mottaker("04036125433", "ove.jonsen#6K5A", new X509Certificate2(), orgNrMottaker.Iso6523());

            //Digital Post
            var digitalPost = new DigitalPost(mottaker, "Ikke-sensitiv tittel");
            
            //Dokumenter
            var hoveddokument = @"C:\sdp\testdata\hoveddokument\hoveddokument.docx";
            var vedlegg = @"C:\sdp\testdata\vedlegg\VedleggsGris.docx";

            //Dokumentpakke
            var dokumentpakke = new Dokumentpakke(new Dokument("Hoveddokument", hoveddokument, "text/docx"));
            dokumentpakke.LeggTilVedlegg(new Dokument("Vedleggsgris",vedlegg,"text/docx","EN"));

            //Forsendelse og sdp-klient
            var forsendelse = new Forsendelse(behandlingsansvarlig, digitalPost, dokumentpakke);
            var manifest = new Manifest(mottaker, behandlingsansvarlig, forsendelse);
            var signatur = new Signatur(certificate);
            var manifestbygger = new ManifestBygger(manifest);
            var signaturbygger = new SignaturBygger(signatur, forsendelse);
            signaturbygger.Bygg();

            //var sikkerDigitalPostKlient = new SikkerDigitalPostKlient(behandlingsansvarlig);
            //sikkerDigitalPostKlient.Send(forsendelse);

        }
    }
}
