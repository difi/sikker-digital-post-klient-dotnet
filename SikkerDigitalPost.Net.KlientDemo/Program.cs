using System.IO;
using System.Security.Cryptography.X509Certificates;
using SikkerDigitalPost.Net.Domene;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.KlientApi;

namespace SikkerDigitalPost.Net.KlientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var nøkkelpar = new Nøkkelpar();
            
            var orgNrAvsender = new Organisasjonsnummer("984661185");
            var behandlingsansvarlig = new Behandlingsansvarlig(orgNrAvsender);
            var tekniskAvsender = new TekniskAvsender(orgNrAvsender, nøkkelpar);
            
            var orgNrMottaker = new Organisasjonsnummer("984661185");
            var mottaker = new Mottaker("04036125433", "ove.jonsen#6K5A", new X509Certificate2(), orgNrMottaker.Iso6523());

            var hoveddokument = "../../../SikkerDigitalPost.Net.Tester/testdata/hoveddokument";
            var digitalPost = new DigitalPost(mottaker, "Ikke-sensitiv tittel");

            var sikkerDigitalPostKlient = new SikkerDigitalPostKlient(tekniskAvsender);
            var forsendelse = new Forsendelse(behandlingsansvarlig, digitalPost, new Dokumentpakke(new Dokument("Hoveddokument",hoveddokument, "text/docx")));
            sikkerDigitalPostKlient.Send(forsendelse);
        }
    }
}
