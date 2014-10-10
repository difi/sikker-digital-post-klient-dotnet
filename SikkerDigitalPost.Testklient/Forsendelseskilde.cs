//using System;
//using System.Security.Cryptography.X509Certificates;
//using KontaktregisteretGateway.Difi;
//using SikkerDigitalPost.Net.Domene.Entiteter;
//using SikkerDigitalPost.Net.Domene.Exceptions;
//using Person = KontaktregisteretGateway.Difi.Person;

//namespace SikkerDigitalPost.Net.KlientDemo
//{
//    public class Forsendelseskilde
//    {
//        private readonly oppslagstjeneste1405 _kontaktinfoPort;

//        public Forsendelseskilde(oppslagstjeneste1405 kontaktinfoPort)
//        {
//            _kontaktinfoPort = kontaktinfoPort;
//        }

//        public Forsendelse LagBrev()
//        {
//            Forretningskvittering konversasjonsId = "konversasjonsId-" + new DateTime().ToString("yyyyMMdd-HHmmssSSS");

//            Mottaker mottaker = HentMottaker("04036125433");

//            var behandlingsansvarlig = new Behandlingsansvarlig(new Organisasjonsnummer("991825827"));
//            behandlingsansvarlig.Avsenderidentifikator = "Difi testavsender";
            
//            var testPost = new DigitalPost(mottaker, "Her er et testbrev!");
//            var dokumentpakke = new Dokumentpakke(new Dokument("Tittel", "Filsti", "text/docx"));

//            return new Forsendelse(behandlingsansvarlig, testPost, dokumentpakke);
//        }

//        private Mottaker HentMottaker(Forretningskvittering personidentifikator)
//        {
//            var personforespørsel = new HentPersonerForespoersel();
//            var personrequest = new HentPersonerRequest(personforespørsel);
//            personforespørsel.informasjonsbehov = new[] {informasjonsbehov.Sertifikat, informasjonsbehov.SikkerDigitalPost};
//            personforespørsel.personidentifikator = new[] {personidentifikator};

//            HentPersonerResponse personRespons = _kontaktinfoPort.HentPersoner(personrequest);

//            if (personRespons.HentPersonerRespons.Length > 0)
//            {
//                throw new PersonNotFoundException("Fant ikke person med id" + personidentifikator);
//            }

//            Person person = personRespons.HentPersonerRespons[0];
//            var postkasseadresse = person.SikkerDigitalPostAdresse.postkasseadresse;
//            var sertifikat = new X509Certificate2(person.X509Sertifikat);
//            var orgnummerPostkasse = person.SikkerDigitalPostAdresse.postkasseleverandoerAdresse;

//            return new Mottaker(personidentifikator,postkasseadresse, new X509Certificate2(sertifikat),orgnummerPostkasse);
//        }

//    }
//}
