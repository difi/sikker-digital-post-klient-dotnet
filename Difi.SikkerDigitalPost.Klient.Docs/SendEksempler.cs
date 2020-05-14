using System;
using System.Security.Cryptography.X509Certificates;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.XmlValidering;

namespace Difi.SikkerDigitalPost.Klient.Docs
{
    public class SendEksempler
    {
        public void DigitalPostSender()
        {
            var personnummer = "01013300002";
            var postkasseadresse = "ola.nordmann#2233";
            var mottaker = new DigitalPostMottaker(
                personnummer 
            );

            var ikkeSensitivTittel = "En tittel som ikke er sensitiv";
            var sikkerhetsnivå = Sikkerhetsnivå.Nivå3;
            var postInfo = new DigitalPostInfo(mottaker, ikkeSensitivTittel, sikkerhetsnivå);
        }

        public void FysiskPostSender()
        {
            var navn = "Ola Nordmann";
            var adresse = new NorskAdresse("0001", "Oslo");
            var mottaker = new FysiskPostMottaker(navn, adresse);

            var returMottaker = new FysiskPostReturmottaker(
                "John Doe", 
                new NorskAdresse("0566", "Oslo")
                {
                    Adresselinje1 = "Returgata 22"
                });

            var fysiskPostMottakerPersonnummer = "27127000293";
            var postInfo = new FysiskPostInfo(
                fysiskPostMottakerPersonnummer,
                mottaker, 
                Posttype.A, 
                Utskriftsfarge.SortHvitt, 
                Posthåndtering.MakuleringMedMelding, 
                returMottaker
            );
        }

        public void OpprettAvsenderOgBehandler()
        {
            var orgnummerAvsender = new Organisasjonsnummer("123456789");
            var avsender = new Avsender(orgnummerAvsender);

            var orgnummerDatabehandler = new Organisasjonsnummer("987654321");
            var databehandler = new Databehandler(orgnummerDatabehandler);
            
            //Hvis man har flere avdelinger innenfor samme organisasjonsnummer, har disse fått unike avsenderidentifikatorer, og kan settes på følgende måte:
            avsender.Avsenderidentifikator = "Avsenderidentifikator.I.Organisasjon";
        }

        public void OpprettForsendelse()
        {
            var hoveddokument = new Dokument(
                tittel: "Dokumenttittel", 
                dokumentsti: "/Dokumenter/Hoveddokument.pdf", 
                mimeType: "application/pdf", 
                språkkode: "NO", 
                filnavn: "filnavn"
            );

            var dokumentpakke = new Dokumentpakke(hoveddokument);

            var vedleggssti = "/Dokumenter/Vedlegg.pdf";
            var vedlegg = new Dokument(
                tittel: "tittel", 
                dokumentsti: vedleggssti, 
                mimeType: "application/pdf", 
                språkkode: "NO", 
                filnavn: "filnavn");

            dokumentpakke.LeggTilVedlegg(vedlegg);

            Avsender avsender = null; //Som initiert tidligere
            PostInfo postInfo = null; //Som initiert tidligere
            var forsendelse = new Forsendelse(avsender, postInfo, dokumentpakke);
        }
        
        public void OpprettForsendelseMedUtvidelse()
        {
            var raw = "<?xml version=\"1.0\" encoding=\"utf-8\"?><lenke xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://begrep.difi.no/sdp/utvidelser/lenke\"><url>https://www.test.no</url><beskrivelse lang=\"nb\">This was raw string</beskrivelse></lenke>";
            
            MetadataDocument metadataDocument = new MetadataDocument("lenke.xml", "application/vnd.difi.dpi.lenke", raw);
            
            Avsender avsender = null; //Som initiert tidligere
            PostInfo postInfo = null; //Som initiert tidligere
            Dokumentpakke dokumentpakke = null; //Som initiert tidligere
            var forsendelse = new Forsendelse(avsender, postInfo, dokumentpakke) { MetadataDocument = metadataDocument };
        }

        public void OpprettKlientOgSendPost()
        {
            var integrasjonsPunktLocalHostMiljø = new Miljø(new Uri("http://127.0.0.1:9093"));
            var klientKonfig = new Klientkonfigurasjon(integrasjonsPunktLocalHostMiljø);

            Databehandler databehandler = null; //Som initiert tidligere
            Forsendelse forsendelse = null;     //Som initiert tidligere

            var sdpKlient = new SikkerDigitalPostKlient(databehandler, klientKonfig);
            var transportkvittering = sdpKlient.Send(forsendelse);

            if (transportkvittering is TransportOkKvittering)
            {
                //Når sending til integrasjonspunkt har gått fint.	
            }
            else if(transportkvittering is TransportFeiletKvittering)
            {
                var beskrivelse = ((TransportFeiletKvittering)transportkvittering).Beskrivelse;
            }
            
            //Hent kvitteringer
            var kvitteringsForespørsel = new Kvitteringsforespørsel();
            Console.WriteLine(" > Henter kvittering på kø...");

            Kvittering kvittering = sdpKlient.HentKvittering(kvitteringsForespørsel);

            if (kvittering is TomKøKvittering)
            {
                Console.WriteLine("  - Kø er tom. Stopper å hente meldinger. ");
            }

            if (kvittering is TransportFeiletKvittering)
            {
                var feil = ((TransportFeiletKvittering) kvittering).Beskrivelse;
                Console.WriteLine("En feil skjedde under transport.");
            }

            if (kvittering is Leveringskvittering)
            {
                Console.WriteLine("  - En leveringskvittering ble hentet!");
            }

            if (kvittering is Åpningskvittering)
            {
                Console.WriteLine("  - Noen har åpnet et brev.");
            }

            if (kvittering is Returpostkvittering)
            {
                Console.WriteLine("  - Du har fått en returpostkvittering for fysisk post.");
            }

            if (kvittering is Mottakskvittering)
            {
                Console.WriteLine("  - Kvittering på sending av fysisk post mottatt.");
            }

            if (kvittering is Feilmelding)
            {
                var feil = (Feilmelding)kvittering;
                Console.WriteLine("  - En feilmelding ble hentet :" + feil.Detaljer, true);
            }
            
            //Husk at det ikke er mulig å hente nye kvitteringer før du har bekreftet mottak av nåværende. 
            sdpKlient.Bekreft((Forretningskvittering)kvittering);
        }
    }
}
