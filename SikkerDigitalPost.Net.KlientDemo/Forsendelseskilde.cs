using System;
using System.Collections.Generic;
using System.Linq;
using KontaktregisteretGateway.Difi;
using SikkerDigitalPost.Net.Domene;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.KlientDemo
{
    public class Forsendelseskilde
    {
        private readonly oppslagstjeneste1405 _kontaktinfoPort;

        public Forsendelseskilde(oppslagstjeneste1405 kontaktinfoPort)
        {
            _kontaktinfoPort = kontaktinfoPort;
        }

        public Forsendelse LagBrev()
        {
            string konversasjonsId = "konversasjonsId-" + new DateTime().ToString("yyyyMMdd-HHmmssSSS");

            Mottaker mottaker = HentMottaker("04036125433");

            var behandlingsansvarlig = new Behandlingsansvarlig(new Organisasjonsnummer("991825827"));
            behandlingsansvarlig.Avsenderidentifikator = "Difi testavsender";
            
            var testPost = new DigitalPost(mottaker, "Her er et testbrev!");
            var dokumentpakke = new Dokumentpakke(new Dokument("Tittel", "Filsti", "text/docx"));

            return new Forsendelse(behandlingsansvarlig, testPost, dokumentpakke);
        }

        private Mottaker HentMottaker(string personidentifikator)
        {
            HentPersonerForespoersel personforespørsel = new HentPersonerForespoersel();
            personforespørsel.informasjonsbehov = new[] {informasjonsbehov.Sertifikat, informasjonsbehov.SikkerDigitalPost};
            personforespørsel.personidentifikator = new[] {personidentifikator};

            //HentPersonerResponse personRespons = _kontaktinfoPort.HentPersoner(personforespørsel);

            return null;

        }

    }
}
