using System.Security.Cryptography.X509Certificates;
using SikkerDigitalPost.Net.Domene;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Klient;

namespace SikkerDigitalPost.Net.KlientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var orgNrAvsender = new Organisasjonsnummer("984661185");
            var orgNrMottaker = new Organisasjonsnummer("984661185");


            // Hent sertifikat etter samtale med Erlend.
            var nøkkelpar = new Nøkkelpar();
            
            var tekniskAvsender = new TekniskAvsender(orgNrAvsender, nøkkelpar);
            var klientkonfigurasjon = new Klientkonfigurasjon();
            var sikkerDigitalPostKlient = new SikkerDigitalPostKlient(tekniskAvsender, klientkonfigurasjon);

            string organisasjonsnummerPostkasse = "9908:984661185";
            var mottaker = new Mottaker("04036125433", "ove.jonsen#6K5A", new X509Certificate2(), orgNrMottaker.Iso6523());
            var digitalPost = new DigitalPost(mottaker, "Ikke-sensitiv tittel");

            var behandlingsansvarlig = new Behandlingsansvarlig(orgNrAvsender);
            var forsendelse = new Forsendelse(behandlingsansvarlig, digitalPost, new Dokumentpakke(new Dokument("Tittel", "Filsti", "text/docx")));

            sikkerDigitalPostKlient.Send(forsendelse);
        }
    }
}
